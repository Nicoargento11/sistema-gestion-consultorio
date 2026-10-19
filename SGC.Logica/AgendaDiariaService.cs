using Microsoft.EntityFrameworkCore;
using SGC.Datos;
using SGC.Entidades;

namespace SGC.Logica;

// Fila de la agenda del medico: el turno mas los datos administrativos del
// paciente que la grilla de "Mi Agenda" muestra. Nunca incluye datos clinicos
// (RNF#07): la historia clinica se consulta en sus propias pantallas.
public class TurnoAgendaItem
{
    public required Turno Turno { get; init; }

    // Recurrente = el paciente ya fue atendido (Asistio) por este medico en un
    // turno anterior; si no, es "Nuevo" para este medico.
    public bool EsRecurrente { get; init; }

    public string HorarioRango => Turno.HorarioRango;
    public string PacienteNombre => Turno.PacienteNombre;
    public string PacienteDni => Turno.PacienteDni;
    public string EstadoAtencion => Turno.EstadoAtencion;
    public EstadoTurno Estado => Turno.Estado;

    public int? Edad => Turno.Paciente?.Edad;
    public string ObraSocial => Turno.Paciente?.ObraSocialNombre ?? "-";
    public string Perfil => EsRecurrente ? "Recurrente" : "Nuevo";

    public bool Atendido => Turno.ActividadMedica?.Activo == true;
}

public class AgendaDiariaService
{
    private readonly TurnoService _turnoService = new();

    // Todos los turnos del dia del medico (incluye cancelados) con edad, obra
    // social y marca nuevo/recurrente. Son 2 consultas en total sin importar
    // la cantidad de turnos: los turnos del dia (con Include) y una sola
    // consulta por lote para saber que pacientes ya fueron atendidos antes.
    public List<TurnoAgendaItem> ObtenerAgenda(int medicoId, DateOnly fecha)
    {
        var turnos = _turnoService.ObtenerPorMedicoYFecha(medicoId, fecha, true);
        var primerasAsistencias = ObtenerPrimerasAsistencias(
            medicoId, turnos.Select(t => t.PacienteId).Distinct().ToList(), fecha);

        return turnos.Select(t => new TurnoAgendaItem
        {
            Turno = t,
            EsRecurrente = primerasAsistencias.TryGetValue(t.PacienteId, out var primera)
                           && primera < t.Fecha.ToDateTime(t.HoraInicio)
        }).ToList();
    }

    // Para cada paciente, fecha y hora de su primera asistencia con este
    // medico (hasta la fecha indicada). Un turno es "recurrente" si esa
    // primera asistencia es anterior al propio turno.
    private static Dictionary<int, DateTime> ObtenerPrimerasAsistencias(int medicoId, List<int> pacienteIds, DateOnly hasta)
    {
        if (pacienteIds.Count == 0)
            return new Dictionary<int, DateTime>();

        using var contexto = SGCContextFactory.Crear();
        var asistencias = contexto.Turnos
            .Where(t => t.MedicoId == medicoId
                        && t.Activo
                        && t.Estado == EstadoTurno.Asistio
                        && t.Fecha <= hasta
                        && pacienteIds.Contains(t.PacienteId))
            .Select(t => new { t.PacienteId, t.Fecha, t.HoraInicio })
            .ToList();

        return asistencias
            .GroupBy(a => a.PacienteId)
            .ToDictionary(g => g.Key, g => g.Min(a => a.Fecha.ToDateTime(a.HoraInicio)));
    }

    // Proximo paciente a atender: primer turno del dia que sigue Pendiente o
    // Confirmado y sin atencion registrada. Si el dia consultado es hoy solo
    // cuentan los turnos con HoraInicio >= ahora; en cualquier otro dia, el
    // primero del dia. Recibe hoy/ahora por parametro para poder probarlo.
    public static TurnoAgendaItem? ElegirProximo(IEnumerable<TurnoAgendaItem> agenda, DateOnly fecha, DateOnly hoy, TimeOnly ahora)
    {
        return agenda
            .Where(i => i.Turno.Fecha == fecha
                        && i.Turno.Activo
                        && (i.Turno.Estado == EstadoTurno.Pendiente || i.Turno.Estado == EstadoTurno.Confirmado)
                        && !i.Atendido
                        && (fecha != hoy || i.Turno.HoraInicio >= ahora))
            .OrderBy(i => i.Turno.HoraInicio)
            .FirstOrDefault();
    }
}
