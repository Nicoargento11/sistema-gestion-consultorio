using Microsoft.EntityFrameworkCore;
using SGC.Datos;
using SGC.Entidades;

namespace SGC.Logica;

public class ActividadMedicaService
{
    public List<TipoActividad> ObtenerTiposActividad()
    {
        using var contexto = SGCContextFactory.Crear();
        return contexto.TiposActividad.Where(t => t.Activo).ToList();
    }

    public List<ActividadMedica> ObtenerTodas(bool incluirInactivas = false)
    {
        using var contexto = SGCContextFactory.Crear();
        IQueryable<ActividadMedica> query = contexto.ActividadesMedicas
            .Include(a => a.TipoActividad)
            .Include(a => a.Turno).ThenInclude(t => t!.Paciente)
            .Include(a => a.Turno).ThenInclude(t => t!.Medico);

        return (incluirInactivas ? query : query.Where(a => a.Activo)).ToList();
    }

    public ActividadMedica? ObtenerPorTurnoId(int turnoId)
    {
        using var contexto = SGCContextFactory.Crear();
        return contexto.ActividadesMedicas
            .Include(a => a.TipoActividad)
            .FirstOrDefault(a => a.Activo && a.TurnoId == turnoId);
    }

    public List<ActividadMedica> ObtenerHistorialPorPaciente(int pacienteId)
    {
        return Consultar(pacienteId: pacienteId);
    }

    public List<ActividadMedica> Consultar(int? pacienteId = null, int? medicoId = null, DateOnly? fecha = null, int? tipoId = null)
    {
        using var contexto = SGCContextFactory.Crear();
        IQueryable<ActividadMedica> query = contexto.ActividadesMedicas
            .Include(a => a.TipoActividad)
            .Include(a => a.Turno).ThenInclude(t => t!.Paciente)
            .Include(a => a.Turno).ThenInclude(t => t!.Medico)
            .Where(a => a.Activo && a.Turno != null);

        if (pacienteId.HasValue)
            query = query.Where(a => a.Turno!.PacienteId == pacienteId.Value);

        if (medicoId.HasValue)
            query = query.Where(a => a.Turno!.MedicoId == medicoId.Value);

        if (fecha.HasValue)
            query = query.Where(a => a.Turno!.Fecha == fecha.Value);

        if (tipoId.HasValue && tipoId.Value > 0)
            query = query.Where(a => a.TipoActividadId == tipoId.Value);

        return query.OrderByDescending(a => a.Turno!.Fecha).ToList();
    }

    public void RegistrarOModificarActividad(Turno turno, int tipoActividadId, string motivo, string procedimiento, string receta)
    {
        if (turno == null)
            throw new ArgumentNullException(nameof(turno), "Debe especificar un turno valido.");

        if (turno.Estado == EstadoTurno.Cancelado)
            throw new InvalidOperationException("No se puede registrar actividad medica en un turno cancelado.");

        if (string.IsNullOrWhiteSpace(motivo))
            throw new ArgumentException("El motivo de la consulta es obligatorio.");

        using var contexto = SGCContextFactory.Crear();

        var tipo = contexto.TiposActividad.FirstOrDefault(t => t.Id == tipoActividadId)
            ?? throw new InvalidOperationException("El tipo de actividad seleccionado no es valido.");

        var existente = contexto.ActividadesMedicas.FirstOrDefault(a => a.Activo && a.TurnoId == turno.Id);

        if (existente != null)
        {
            existente.TipoActividadId = tipoActividadId;
            existente.MotivoConsulta = motivo.Trim();
            existente.Procedimiento = procedimiento?.Trim() ?? string.Empty;
            existente.RecetaMedicamentos = receta?.Trim() ?? string.Empty;
        }
        else
        {
            contexto.ActividadesMedicas.Add(new ActividadMedica
            {
                TurnoId = turno.Id,
                TipoActividadId = tipoActividadId,
                MotivoConsulta = motivo.Trim(),
                Procedimiento = procedimiento?.Trim() ?? string.Empty,
                RecetaMedicamentos = receta?.Trim() ?? string.Empty,
                Activo = true
            });
        }

        contexto.SaveChanges();
    }

    public void EliminarLogico(int actividadId)
    {
        using var contexto = SGCContextFactory.Crear();
        var actividad = contexto.ActividadesMedicas.FirstOrDefault(a => a.Id == actividadId)
            ?? throw new InvalidOperationException("La actividad medica no existe.");

        actividad.Activo = false;
        contexto.SaveChanges();
    }
}
