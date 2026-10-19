using Microsoft.EntityFrameworkCore;
using SGC.Datos;
using SGC.Entidades;

namespace SGC.Logica;

public class ExcepcionAgendaService
{
    public List<ExcepcionAgenda> ObtenerTodos()
    {
        using var contexto = SGCContextFactory.Crear();
        return contexto.ExcepcionesAgenda.Where(e => e.Activo).Include(e => e.Medico).ToList();
    }

    // Listado para la grilla: cada excepcion con su estado respecto de hoy y
    // la cantidad de turnos activos (no cancelados) que todavia se
    // superponen con ella. Los turnos de todas las excepciones se traen con
    // UNA sola consulta (medicos y rango de fechas involucrados) y se cuentan
    // en memoria, en vez de una consulta por fila.
    public List<ExcepcionFila> ObtenerListado(DateOnly? hoy = null)
    {
        var dia = hoy ?? DateOnly.FromDateTime(DateTime.Today);

        using var contexto = SGCContextFactory.Crear();
        var excepciones = contexto.ExcepcionesAgenda
            .AsNoTracking()
            .Where(e => e.Activo)
            .Include(e => e.Medico)
            .ToList();

        if (excepciones.Count == 0) return new List<ExcepcionFila>();

        var medicoIds = excepciones.Select(e => e.MedicoId).Distinct().ToList();
        var desde = excepciones.Min(e => e.Fecha);
        var hasta = excepciones.Max(e => e.Fecha);

        var turnos = contexto.Turnos
            .AsNoTracking()
            .Where(t => t.Activo && t.Estado != EstadoTurno.Cancelado
                        && medicoIds.Contains(t.MedicoId) && t.Fecha >= desde && t.Fecha <= hasta)
            .Select(t => new { t.MedicoId, t.Fecha, t.HoraInicio, t.DuracionMinutos })
            .ToList()
            .ToLookup(t => (t.MedicoId, t.Fecha));

        return excepciones
            .Select(e => new ExcepcionFila
            {
                Excepcion = e,
                Estado = ExcepcionFila.CalcularEstado(e.Fecha, dia),
                TurnosAfectados = turnos[(e.MedicoId, e.Fecha)].Count(t =>
                    e.Tipo == TipoExcepcionAgenda.DiaCompleto
                    || (e.HoraInicio != null && e.HoraFin != null
                        && t.HoraInicio < e.HoraFin && e.HoraInicio < t.HoraInicio.AddMinutes(t.DuracionMinutos)))
            })
            // Vigentes y proximas primero (la mas cercana arriba), luego las pasadas (la mas reciente arriba).
            .OrderBy(f => f.Estado == EstadoExcepcion.Pasada ? 1 : 0)
            .ThenBy(f => f.Estado == EstadoExcepcion.Pasada ? 0 : f.Fecha.DayNumber)
            .ThenByDescending(f => f.Estado == EstadoExcepcion.Pasada ? f.Fecha.DayNumber : 0)
            .ThenBy(f => f.Id)
            .ToList();
    }

    public List<ExcepcionAgenda> ObtenerPorMedico(int medicoId)
    {
        using var contexto = SGCContextFactory.Crear();
        return contexto.ExcepcionesAgenda
            .Where(e => e.Activo && e.MedicoId == medicoId)
            .Include(e => e.Medico)
            .ToList();
    }

    public ExcepcionAgenda? ObtenerPorId(int id)
    {
        using var contexto = SGCContextFactory.Crear();
        return contexto.ExcepcionesAgenda.Include(e => e.Medico).FirstOrDefault(e => e.Id == id);
    }

    public List<Turno> Agregar(ExcepcionAgenda excepcion)
    {
        using var contexto = SGCContextFactory.Crear();
        var medico = Validar(excepcion, contexto);

        excepcion.Id = 0;
        excepcion.Activo = true;
        excepcion.Medico = medico;
        contexto.ExcepcionesAgenda.Add(excepcion);
        contexto.SaveChanges();

        return CancelarTurnosAfectados(excepcion, contexto);
    }

    // Al cargar una ausencia, los turnos que ya estaban agendados en ese
    // rango dejan de tener sentido: se cancelan y se avisa al paciente
    // (mismo patron que usaba FormHorarios al eliminar un bloque de horario).
    private List<Turno> CancelarTurnosAfectados(ExcepcionAgenda excepcion, SGCContext contexto)
    {
        var notificacionService = new NotificacionService();

        var turnosAfectados = contexto.Turnos
            .Include(t => t.Paciente)
            .Where(t => t.Activo && t.MedicoId == excepcion.MedicoId && t.Fecha == excepcion.Fecha)
            .Where(t => t.Estado == EstadoTurno.Pendiente || t.Estado == EstadoTurno.Confirmado)
            .AsEnumerable()
            .Where(t => excepcion.Tipo == TipoExcepcionAgenda.DiaCompleto || SeSuperponeConRango(t, excepcion))
            .ToList();

        foreach (var turno in turnosAfectados)
        {
            turno.Estado = EstadoTurno.Cancelado;
            turno.Activo = false;

            if (turno.Paciente != null)
            {
                notificacionService.AvisarTurno(turno.Paciente, "Cancelacion",
                    $"Se cancelo su turno del {turno.Fecha:dd/MM/yyyy} con el Dr./Dra. {excepcion.Medico?.Apellido} por ausencia del profesional.");
            }
        }

        contexto.SaveChanges();
        return turnosAfectados;
    }

    private static bool SeSuperponeConRango(Turno turno, ExcepcionAgenda excepcion)
    {
        if (excepcion.HoraInicio == null || excepcion.HoraFin == null)
            return false;

        return turno.HoraInicio < excepcion.HoraFin && excepcion.HoraInicio < turno.HoraFin;
    }

    public void Modificar(ExcepcionAgenda excepcion)
    {
        using var contexto = SGCContextFactory.Crear();
        var medico = Validar(excepcion, contexto);

        var existente = contexto.ExcepcionesAgenda.FirstOrDefault(e => e.Id == excepcion.Id)
            ?? throw new InvalidOperationException("La excepcion que intenta modificar no existe.");

        existente.MedicoId = excepcion.MedicoId;
        existente.Medico = medico;
        existente.Fecha = excepcion.Fecha;
        existente.Tipo = excepcion.Tipo;
        existente.HoraInicio = excepcion.HoraInicio;
        existente.HoraFin = excepcion.HoraFin;
        existente.Motivo = excepcion.Motivo;
        contexto.SaveChanges();
    }

    public void EliminarLogico(int id)
    {
        using var contexto = SGCContextFactory.Crear();
        var excepcion = contexto.ExcepcionesAgenda.FirstOrDefault(e => e.Id == id)
            ?? throw new InvalidOperationException("La excepcion que intenta eliminar no existe.");

        excepcion.Activo = false;
        contexto.SaveChanges();
    }

    private Medico Validar(ExcepcionAgenda excepcion, SGCContext contexto)
    {
        var medico = contexto.Medicos.FirstOrDefault(m => m.Id == excepcion.MedicoId);
        if (medico is null || !medico.Activo)
            throw new ArgumentException("Debe seleccionar un medico valido.");

        if (excepcion.Fecha < DateOnly.FromDateTime(DateTime.Today))
            throw new ArgumentException("No se puede cargar una excepcion en una fecha pasada.");

        if (string.IsNullOrWhiteSpace(excepcion.Motivo))
            throw new ArgumentException("Debe indicar el motivo de la ausencia.");

        if (excepcion.Tipo == TipoExcepcionAgenda.RangoHorario)
        {
            if (excepcion.HoraInicio is null || excepcion.HoraFin is null)
                throw new ArgumentException("Debe indicar hora de inicio y fin para una excepcion de rango horario.");

            if (excepcion.HoraFin <= excepcion.HoraInicio)
                throw new ArgumentException("La hora de fin debe ser posterior a la hora de inicio.");
        }

        return medico;
    }
}
