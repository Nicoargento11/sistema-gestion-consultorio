using Microsoft.EntityFrameworkCore;
using SGC.Datos;
using SGC.Entidades;

namespace SGC.Logica;

public class AgendaMedicoService
{
    public List<AgendaMedico> ObtenerTodos()
    {
        using var contexto = SGCContextFactory.Crear();
        return contexto.AgendasMedico
            .Where(a => a.Activo && a.Medico!.Activo)
            .Include(a => a.Medico)
            .ToList();
    }

    // Vista semanal: una fila por medico activo (tenga o no horarios) con sus
    // franjas activas y las horas semanales. Dos consultas en total.
    public List<AgendaSemanalFila> ObtenerResumenSemanal()
    {
        using var contexto = SGCContextFactory.Crear();

        var medicos = contexto.Medicos
            .AsNoTracking()
            .Where(m => m.Activo)
            .OrderBy(m => m.Apellido).ThenBy(m => m.Nombre)
            .ToList();

        var franjas = contexto.AgendasMedico
            .AsNoTracking()
            .Where(a => a.Activo)
            .ToList()
            .ToLookup(a => a.MedicoId);

        return medicos
            .Select(m => new AgendaSemanalFila { Medico = m, Franjas = franjas[m.Id].ToList() })
            .ToList();
    }

    public List<AgendaMedico> ObtenerPorMedico(int medicoId)
    {
        using var contexto = SGCContextFactory.Crear();
        return contexto.AgendasMedico
            .Where(a => a.Activo && a.MedicoId == medicoId)
            .Include(a => a.Medico)
            .ToList();
    }

    public AgendaMedico? ObtenerPorId(int id)
    {
        using var contexto = SGCContextFactory.Crear();
        return contexto.AgendasMedico.Include(a => a.Medico).FirstOrDefault(a => a.Id == id);
    }

    public List<AgendaMedico> ObtenerPorMedicoYDia(int medicoId, DayOfWeek dia)
    {
        using var contexto = SGCContextFactory.Crear();
        return contexto.AgendasMedico
            .Where(a => a.Activo && a.MedicoId == medicoId && a.DiaSemana == dia)
            .ToList();
    }

    // Agenda del medico para una fecha concreta: sus bloques de ese dia de la
    // semana, los dias en que atiende y las ausencias (ExcepcionAgenda) de esa
    // fecha. Solo lectura; no modifica las reglas de AsignarTurno/MedicoAtiende.
    public DisponibilidadDia ObtenerDisponibilidad(int medicoId, DateOnly fecha)
    {
        using var contexto = SGCContextFactory.Crear();

        var franjas = contexto.AgendasMedico
            .AsNoTracking()
            .Where(a => a.Activo && a.MedicoId == medicoId)
            .ToList();

        var ausencias = contexto.ExcepcionesAgenda
            .AsNoTracking()
            .Where(e => e.Activo && e.MedicoId == medicoId && e.Fecha == fecha)
            .ToList();

        return new DisponibilidadDia
        {
            Fecha = fecha,
            Bloques = franjas.Where(a => a.DiaSemana == fecha.DayOfWeek).OrderBy(a => a.HoraInicio).ToList(),
            Ausencias = ausencias,
            DiasQueAtiende = franjas.Select(a => a.DiaSemana).Distinct()
                .OrderBy(d => ((int)d + 6) % 7).ToList()
        };
    }

    public bool MedicoAtiende(int medicoId, DayOfWeek dia, TimeOnly horaInicioTurno, int duracionMinutos)
    {
        if (duracionMinutos <= 0) duracionMinutos = 30;

        // En minutos: horaInicio + duracion con TimeOnly da la vuelta pasada
        // la medianoche (23:30 + 45 min = 00:15) y pasaria el chequeo.
        int inicio = Horas.Minutos(horaInicioTurno);
        int fin = inicio + duracionMinutos;

        using var contexto = SGCContextFactory.Crear();
        return contexto.AgendasMedico
            .Where(a => a.Activo && a.MedicoId == medicoId && a.DiaSemana == dia)
            .AsEnumerable()
            .Any(a => inicio >= Horas.Minutos(a.HoraInicio) && fin <= Horas.Minutos(a.HoraFin));
    }

    public void Agregar(AgendaMedico agenda)
    {
        using var contexto = SGCContextFactory.Crear();
        var medico = ValidarDatosBasicos(agenda, contexto);
        ValidarSuperposicion(agenda, idAExcluir: null, contexto);

        // El indice unico incluye franjas dadas de baja: si hay una identica,
        // se vuelve a activar en lugar de insertar un duplicado.
        var inactiva = contexto.AgendasMedico.FirstOrDefault(a => !a.Activo
            && a.MedicoId == agenda.MedicoId && a.DiaSemana == agenda.DiaSemana
            && a.HoraInicio == agenda.HoraInicio && a.HoraFin == agenda.HoraFin);
        if (inactiva != null)
        {
            inactiva.Activo = true;
            Persistencia.Guardar(contexto);
            return;
        }

        agenda.Id = 0;
        agenda.Activo = true;
        agenda.Medico = medico;
        contexto.AgendasMedico.Add(agenda);
        Persistencia.Guardar(contexto);
    }

    public void Modificar(AgendaMedico agenda)
    {
        using var contexto = SGCContextFactory.Crear();
        var medico = ValidarDatosBasicos(agenda, contexto);

        var existente = contexto.AgendasMedico.FirstOrDefault(a => a.Id == agenda.Id)
            ?? throw new InvalidOperationException("El registro de agenda que intenta modificar no existe.");

        ValidarSuperposicion(agenda, idAExcluir: existente.Id, contexto);

        if (contexto.AgendasMedico.Any(a => !a.Activo && a.Id != existente.Id
                && a.MedicoId == agenda.MedicoId && a.DiaSemana == agenda.DiaSemana
                && a.HoraInicio == agenda.HoraInicio && a.HoraFin == agenda.HoraFin))
            throw new InvalidOperationException(
                "Ya existe una franja idéntica para ese médico y día que fue dada de baja. Elimine esta franja y agregue la nueva para reactivarla.");

        // La franja vieja desaparece; la nueva solo la reemplaza si es del
        // mismo medico y dia.
        var reemplazo = agenda.MedicoId == existente.MedicoId && agenda.DiaSemana == existente.DiaSemana
            ? agenda : null;
        ExigirSinTurnosHuerfanos(contexto, existente, reemplazo);

        existente.MedicoId = agenda.MedicoId;
        existente.Medico = medico;
        existente.DiaSemana = agenda.DiaSemana;
        existente.HoraInicio = agenda.HoraInicio;
        existente.HoraFin = agenda.HoraFin;
        Persistencia.Guardar(contexto);
    }

    public void EliminarLogico(int id)
    {
        using var contexto = SGCContextFactory.Crear();
        var agenda = contexto.AgendasMedico.FirstOrDefault(a => a.Id == id)
            ?? throw new InvalidOperationException("El registro de agenda que intenta eliminar no existe.");

        ExigirSinTurnosHuerfanos(contexto, agenda, reemplazo: null);

        agenda.Activo = false;
        contexto.SaveChanges();
    }

    // Rechaza el cambio si dejaria turnos confirmados futuros del medico, que
    // hoy caen dentro de la franja vieja, fuera de todas las otras franjas
    // activas del mismo medico y dia (mas el reemplazo, si lo hay).
    private static void ExigirSinTurnosHuerfanos(SGCContext contexto, AgendaMedico vieja, AgendaMedico? reemplazo)
    {
        var restantes = contexto.AgendasMedico
            .Where(a => a.Activo && a.MedicoId == vieja.MedicoId && a.DiaSemana == vieja.DiaSemana && a.Id != vieja.Id)
            .ToList();
        if (reemplazo != null) restantes.Add(reemplazo);

        int viejaIni = Horas.Minutos(vieja.HoraInicio), viejaFin = Horas.Minutos(vieja.HoraFin);

        int afectados = TurnosFuturos.Confirmados(contexto)
            .Where(t => t.MedicoId == vieja.MedicoId)
            .AsEnumerable()
            .Where(t => t.Fecha.DayOfWeek == vieja.DiaSemana)
            .Count(t =>
            {
                int ini = Horas.Minutos(t.HoraInicio), fin = ini + t.DuracionMinutos;
                bool dentroDeLaVieja = ini >= viejaIni && fin <= viejaFin;
                return dentroDeLaVieja && !restantes.Any(a => ini >= Horas.Minutos(a.HoraInicio) && fin <= Horas.Minutos(a.HoraFin));
            });

        if (afectados > 0)
            throw new InvalidOperationException(
                $"No se puede modificar o eliminar esta franja: dejaría sin horario de atención a {afectados} turno(s) confirmado(s) futuro(s). Cancele o reasigne esos turnos primero.");
    }

    private void ValidarSuperposicion(AgendaMedico agenda, int? idAExcluir, SGCContext contexto)
    {
        var franjasDelDia = contexto.AgendasMedico
            .Where(a => a.Activo && a.MedicoId == agenda.MedicoId && a.DiaSemana == agenda.DiaSemana && a.Id != (idAExcluir ?? 0))
            .ToList();

        foreach (var a in franjasDelDia)
        {
            if (a.HoraInicio < agenda.HoraFin && agenda.HoraInicio < a.HoraFin)
            {
                throw new InvalidOperationException("El rango horario seleccionado se superpone con otro registro de agenda para el mismo medico y dia de semana.");
            }
        }
    }

    private Medico ValidarDatosBasicos(AgendaMedico agenda, SGCContext contexto)
    {
        var medico = contexto.Medicos.FirstOrDefault(m => m.Id == agenda.MedicoId);
        if (medico is null || !medico.Activo)
            throw new ArgumentException("Debe seleccionar un medico valido.");

        if (agenda.HoraFin <= agenda.HoraInicio)
            throw new ArgumentException("La hora de fin debe ser posterior a la hora de inicio.");

        return medico;
    }
}
