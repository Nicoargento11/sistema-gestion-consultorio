using SGC.Entidades;

namespace SGC.Logica;

public class ActividadMedicaService
{
    private static readonly List<TipoActividad> _tiposActividad = new()
    {
        new TipoActividad { Id = 1, NombreTipo = "Consulta General", Descripcion = "Atencion clinica de rutina o primera vez", Activo = true },
        new TipoActividad { Id = 2, NombreTipo = "Control / Seguimiento", Descripcion = "Control periodico o post-tratamiento", Activo = true },
        new TipoActividad { Id = 3, NombreTipo = "Estudio / Practica", Descripcion = "Realizacion o evaluacion de estudios clinicos", Activo = true },
        new TipoActividad { Id = 4, NombreTipo = "Receta / Prescripcion", Descripcion = "Emision o renovacion de recetas farmacologicas", Activo = true },
        new TipoActividad { Id = 5, NombreTipo = "Certificado Medico", Descripcion = "Emision de apto fisico o certificado medico", Activo = true }
    };

    private static readonly List<ActividadMedica> _actividades = new()
    {
        new ActividadMedica
        {
            Id = 1,
            TurnoId = 1,
            TipoActividadId = 1,
            TipoActividad = _tiposActividad[0],
            MotivoConsulta = "Control clinico general y chequeo anual de rutina.",
            Procedimiento = "Examen fisico completo: presion arterial 120/80 mmHg, auscultacion cardiaca y respiratoria normal. Sin hallazgos patologicos.",
            RecetaMedicamentos = "Solicitud de laboratorio de sangre y orina completo de rutina.",
            Activo = true
        }
    };
    private static int _siguienteId = 2;

    static ActividadMedicaService()
    {
        var turnoService = new TurnoService();
        var turno1 = turnoService.ObtenerPorId(1);
        if (turno1 != null && _actividades.Count > 0)
        {
            _actividades[0].Turno = turno1;
            turno1.ActividadMedica = _actividades[0];
        }
    }

    public List<TipoActividad> ObtenerTiposActividad()
    {
        return _tiposActividad.Where(t => t.Activo).ToList();
    }

    public List<ActividadMedica> ObtenerTodas(bool incluirInactivas = false)
    {
        return (incluirInactivas ? _actividades : _actividades.Where(a => a.Activo)).ToList();
    }

    public ActividadMedica? ObtenerPorTurnoId(int turnoId)
    {
        return _actividades.FirstOrDefault(a => a.Activo && a.TurnoId == turnoId);
    }

    public List<ActividadMedica> ObtenerHistorialPorPaciente(int pacienteId)
    {
        return _actividades
            .Where(a => a.Activo && a.Turno != null && a.Turno.PacienteId == pacienteId)
            .OrderByDescending(a => a.Turno!.Fecha)
            .ToList();
    }

    public void RegistrarOModificarActividad(Turno turno, int tipoActividadId, string motivo, string procedimiento, string receta)
    {
        if (turno == null)
            throw new ArgumentNullException(nameof(turno), "Debe especificar un turno valido.");

        if (turno.Estado == EstadoTurno.Cancelado)
            throw new InvalidOperationException("No se puede registrar actividad medica en un turno cancelado.");

        if (string.IsNullOrWhiteSpace(motivo))
            throw new ArgumentException("El motivo de la consulta es obligatorio.");

        var tipo = _tiposActividad.FirstOrDefault(t => t.Id == tipoActividadId)
            ?? throw new InvalidOperationException("El tipo de actividad seleccionado no es valido.");

        var existente = _actividades.FirstOrDefault(a => a.Activo && a.TurnoId == turno.Id);

        if (existente != null)
        {
            existente.TipoActividadId = tipoActividadId;
            existente.TipoActividad = tipo;
            existente.MotivoConsulta = motivo.Trim();
            existente.Procedimiento = procedimiento?.Trim() ?? string.Empty;
            existente.RecetaMedicamentos = receta?.Trim() ?? string.Empty;
        }
        else
        {
            var nuevaActividad = new ActividadMedica
            {
                Id = _siguienteId++,
                TurnoId = turno.Id,
                Turno = turno,
                TipoActividadId = tipoActividadId,
                TipoActividad = tipo,
                MotivoConsulta = motivo.Trim(),
                Procedimiento = procedimiento?.Trim() ?? string.Empty,
                RecetaMedicamentos = receta?.Trim() ?? string.Empty,
                Activo = true
            };

            _actividades.Add(nuevaActividad);
            turno.ActividadMedica = nuevaActividad;
        }
    }

    public void EliminarLogico(int actividadId)
    {
        var actividad = _actividades.FirstOrDefault(a => a.Id == actividadId)
            ?? throw new InvalidOperationException("La actividad medica no existe.");

        actividad.Activo = false;
        if (actividad.Turno != null && actividad.Turno.ActividadMedica?.Id == actividadId)
        {
            actividad.Turno.ActividadMedica = null;
        }
    }
}
