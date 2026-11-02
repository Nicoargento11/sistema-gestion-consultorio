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
                        && Horas.Superpone(Horas.Minutos(t.HoraInicio), Horas.Minutos(t.HoraInicio) + t.DuracionMinutos,
                                           Horas.Minutos(e.HoraInicio.Value), Horas.Minutos(e.HoraFin.Value))))
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

        // La ausencia y la cancelacion de los turnos afectados se guardan
        // juntas (un unico SaveChanges): si algo falla no queda una ausencia
        // guardada con un error en pantalla.
        var turnosAfectados = CancelarTurnosAfectados(excepcion, contexto);
        contexto.SaveChanges();

        // Los avisos se registran recien con todo guardado.
        AvisarCancelaciones(turnosAfectados, excepcion);
        return turnosAfectados;
    }

    // Al cargar una ausencia, los turnos que ya estaban agendados en ese
    // rango dejan de tener sentido: se cancelan y se avisa al paciente
    // (mismo patron que usaba FormHorarios al eliminar un bloque de horario).
    // Solo marca los turnos como cancelados en el contexto; no guarda ni avisa.
    private List<Turno> CancelarTurnosAfectados(ExcepcionAgenda excepcion, SGCContext contexto)
    {
        var turnosAfectados = contexto.Turnos
            .Include(t => t.Paciente)
            .Where(t => t.Activo && t.MedicoId == excepcion.MedicoId && t.Fecha == excepcion.Fecha)
            .Where(t => t.Estado == EstadoTurno.Confirmado)
            .AsEnumerable()
            .Where(t => excepcion.Tipo == TipoExcepcionAgenda.DiaCompleto || SeSuperponeConRango(t, excepcion))
            .ToList();

        foreach (var turno in turnosAfectados)
        {
            turno.Estado = EstadoTurno.Cancelado;
            turno.Activo = false;
        }

        return turnosAfectados;
    }

    private static void AvisarCancelaciones(List<Turno> turnosCancelados, ExcepcionAgenda excepcion)
    {
        var notificacionService = new NotificacionService();

        foreach (var turno in turnosCancelados.Where(t => t.Paciente != null))
        {
            notificacionService.AvisarTurno(turno.Paciente!, "Cancelacion",
                $"Se cancelo su turno del {turno.Fecha:dd/MM/yyyy} con el Dr./Dra. {excepcion.Medico?.Apellido} por ausencia del profesional.");
        }
    }

    private static bool SeSuperponeConRango(Turno turno, ExcepcionAgenda excepcion)
    {
        if (excepcion.HoraInicio == null || excepcion.HoraFin == null)
            return false;

        int inicioTurno = Horas.Minutos(turno.HoraInicio);
        return Horas.Superpone(inicioTurno, inicioTurno + turno.DuracionMinutos,
            Horas.Minutos(excepcion.HoraInicio.Value), Horas.Minutos(excepcion.HoraFin.Value));
    }

    public void Modificar(ExcepcionAgenda excepcion)
    {
        using var contexto = SGCContextFactory.Crear();

        var existente = contexto.ExcepcionesAgenda.FirstOrDefault(e => e.Id == excepcion.Id)
            ?? throw new InvalidOperationException("La excepcion que intenta modificar no existe.");

        var medico = Validar(excepcion, contexto, existente);

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

    // existente = la ausencia tal como esta guardada (solo al modificar). Si el
    // rango (medico, fecha, tipo, horas) no cambia, no se exige que sea futuro
    // ni se revisan superposiciones: permite corregir el motivo de una
    // ausencia que ya paso.
    private Medico Validar(ExcepcionAgenda excepcion, SGCContext contexto, ExcepcionAgenda? existente = null)
    {
        var medico = contexto.Medicos.FirstOrDefault(m => m.Id == excepcion.MedicoId);
        if (medico is null || !medico.Activo)
            throw new ArgumentException("Debe seleccionar un médico válido.");

        excepcion.Motivo = Texto.Normalizar(excepcion.Motivo);
        if (excepcion.Motivo.Length == 0)
            throw new ArgumentException("Debe indicar el motivo de la ausencia.");

        if (excepcion.Motivo.Length > 200)
            throw new ArgumentException("El motivo no puede superar los 200 caracteres.");

        if (excepcion.Tipo == TipoExcepcionAgenda.RangoHorario)
        {
            if (excepcion.HoraInicio is null || excepcion.HoraFin is null)
                throw new ArgumentException("Debe indicar hora de inicio y fin para una excepción de rango horario.");

            if (excepcion.HoraFin <= excepcion.HoraInicio)
                throw new ArgumentException("La hora de fin debe ser posterior a la hora de inicio.");
        }
        else
        {
            // Dia completo no lleva horas (lo exige una restriccion de la base).
            excepcion.HoraInicio = null;
            excepcion.HoraFin = null;
        }

        bool rangoCambiado = existente is null
            || existente.MedicoId != excepcion.MedicoId
            || existente.Fecha != excepcion.Fecha
            || existente.Tipo != excepcion.Tipo
            || existente.HoraInicio != excepcion.HoraInicio
            || existente.HoraFin != excepcion.HoraFin;

        if (rangoCambiado)
        {
            if (excepcion.Fecha < DateOnly.FromDateTime(DateTime.Today))
                throw new ArgumentException("No se puede cargar una excepción en una fecha pasada.");

            ExigirSinSuperposicion(excepcion, contexto);
        }

        return medico;
    }

    // Dos ausencias activas del mismo medico y fecha no pueden superponerse
    // (un dia completo se superpone con cualquier otra del mismo dia).
    private static void ExigirSinSuperposicion(ExcepcionAgenda excepcion, SGCContext contexto)
    {
        var delDia = contexto.ExcepcionesAgenda
            .AsNoTracking()
            .Where(e => e.Activo && e.MedicoId == excepcion.MedicoId && e.Fecha == excepcion.Fecha && e.Id != excepcion.Id)
            .ToList();

        foreach (var otra in delDia)
        {
            bool alguna = otra.Tipo == TipoExcepcionAgenda.DiaCompleto || excepcion.Tipo == TipoExcepcionAgenda.DiaCompleto
                || Horas.Superpone(Horas.Minutos(excepcion.HoraInicio!.Value), Horas.Minutos(excepcion.HoraFin!.Value),
                                   Horas.Minutos(otra.HoraInicio!.Value), Horas.Minutos(otra.HoraFin!.Value));
            if (alguna)
                throw new InvalidOperationException(
                    $"El médico ya tiene una ausencia cargada el {excepcion.Fecha:dd/MM/yyyy} ({Describir(otra)}: {otra.Motivo}) que se superpone con esta.");
        }
    }

    internal static string Describir(ExcepcionAgenda e)
        => e.Tipo == TipoExcepcionAgenda.DiaCompleto ? "todo el día" : $"de {e.HoraInicio:HH:mm} a {e.HoraFin:HH:mm}";

    // Ausencia activa del medico que se superpone con el horario dado (dia
    // completo, o rango horario que se cruza con el turno); null si no hay.
    internal static ExcepcionAgenda? BuscarAusencia(SGCContext contexto, int medicoId, DateOnly fecha, TimeOnly horaInicio, int duracionMinutos)
    {
        int inicio = Horas.Minutos(horaInicio), fin = inicio + duracionMinutos;

        return contexto.ExcepcionesAgenda
            .Where(e => e.Activo && e.MedicoId == medicoId && e.Fecha == fecha)
            .AsEnumerable()
            .FirstOrDefault(e => e.Tipo == TipoExcepcionAgenda.DiaCompleto
                || (e.HoraInicio != null && e.HoraFin != null
                    && Horas.Superpone(inicio, fin, Horas.Minutos(e.HoraInicio.Value), Horas.Minutos(e.HoraFin.Value))));
    }
}
