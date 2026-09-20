using SGC.Entidades;

namespace SGC.Logica;

public class ExcepcionAgendaService
{
    private static readonly List<ExcepcionAgenda> _excepciones = new();
    private static int _siguienteId = 1;

    private readonly MedicoService _medicoService = new();
    private readonly TurnoService _turnoService = new();
    private readonly NotificacionService _notificacionService = new();

    public List<ExcepcionAgenda> ObtenerTodos()
    {
       return _excepciones.Where(e => e.Activo).Select(ResolverNavegacion).ToList();
    }

    public List<ExcepcionAgenda> ObtenerPorMedico(int medicoId)
    {
        return _excepciones.Where(e => e.Activo && e.MedicoId == medicoId).Select(ResolverNavegacion).ToList();
    }

    public ExcepcionAgenda? ObtenerPorId(int id)
    {
        var excepcion = _excepciones.FirstOrDefault(e => e.Id == id);
        return excepcion is null ? null : ResolverNavegacion(excepcion);
    }

    public List<Turno> Agregar(ExcepcionAgenda excepcion)
    {
        var medico = Validar(excepcion);

        excepcion.Id = _siguienteId++;
        excepcion.Activo = true;
        excepcion.Medico = medico;
        _excepciones.Add(excepcion);

        return CancelarTurnosAfectados(excepcion);
    }

    private List<Turno> CancelarTurnosAfectados(ExcepcionAgenda excepcion)
    {
        var turnosAfectados = _turnoService.ObtenerTodos(false, excepcion.MedicoId, excepcion.Fecha)
            .Where(t => t.Estado is EstadoTurno.Pendiente or EstadoTurno.Confirmado)
            .Where(t => excepcion.Tipo == TipoExcepcionAgenda.DiaCompleto || SeSuperponeConRango(t, excepcion))
            .ToList();

        foreach (var turno in turnosAfectados)
        {
            _turnoService.CancelarTurno(turno.Id);

            if (turno.Paciente != null)
            {
                _notificacionService.AvisarTurno(turno.Paciente, "Cancelacion",
                    $"Se cancelo su turno del {turno.Fecha:dd/MM/yyyy} con el Dr./Dra. {excepcion.Medico?.Apellido} por ausencia del profesional.");
            }
        }

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
        var medico = Validar(excepcion);

        var existente = _excepciones.FirstOrDefault(e => e.Id == excepcion.Id)
            ?? throw new InvalidOperationException("La excepcion que intenta modificar no existe.");

        existente.MedicoId = excepcion.MedicoId;
        existente.Medico = medico;
        existente.Fecha = excepcion.Fecha;
        existente.Tipo = excepcion.Tipo;
        existente.HoraInicio = excepcion.HoraInicio;
        existente.HoraFin = excepcion.HoraFin;
        existente.Motivo = excepcion.Motivo;
    }

    public void EliminarLogico(int id)
    {
        var excepcion = _excepciones.FirstOrDefault(e => e.Id == id)
            ?? throw new InvalidOperationException("La excepcion que intenta eliminar no existe.");

        excepcion.Activo = false;
    }

    private ExcepcionAgenda ResolverNavegacion(ExcepcionAgenda excepcion)
    {
        excepcion.Medico = _medicoService.ObtenerPorId(excepcion.MedicoId);
        return excepcion;
    }

    private Medico Validar(ExcepcionAgenda excepcion)
    {
        var medico = _medicoService.ObtenerPorId(excepcion.MedicoId);
        if (medico is null || !medico.Activo)
            throw new ArgumentException("Debe seleccionar un medico valido.");

        if (excepcion.Fecha < DateOnly.FromDateTime(DateTime.Today))
            throw new ArgumentException("No se puede cargar una excepcion en una fecha pasada.");

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
