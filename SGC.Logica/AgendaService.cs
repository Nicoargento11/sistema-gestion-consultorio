using SGC.Entidades;

namespace SGC.Logica;

public class AgendaService
{
    private static readonly List<AgendaMedico> _agendas = new();
    private static int _siguienteId = 1;
    private static bool _semillaCargada;

    public AgendaService()
    {
        AsegurarSemilla();
    }

    private static void AsegurarSemilla()
    {
        if (_semillaCargada) return;

        var medicoService = new MedicoService();
        var medicos = medicoService.ObtenerTodos();

        var diasHabiles = new[]
        {
            DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday
        };

        foreach (var medico in medicos)
        {
            foreach (var dia in diasHabiles)
            {
                _agendas.Add(new AgendaMedico
                {
                    Id = _siguienteId++,
                    MedicoId = medico.Id,
                    Medico = medico,
                    HoraInicio = new TimeOnly(9, 0),
                    HoraFin = new TimeOnly(11, 30),
                    DiaSemana = dia,
                    Activo = true
                });
            }
        }

        _semillaCargada = true;
    }

    public List<AgendaMedico> Consultar(int? medicoId = null, DayOfWeek? dia = null)
    {
        IEnumerable<AgendaMedico> query = _agendas.Where(a => a.Activo);

        if (medicoId.HasValue)
            query = query.Where(a => a.MedicoId == medicoId.Value);

        if (dia.HasValue)
            query = query.Where(a => a.DiaSemana == dia.Value);

        return query
            .OrderBy(a => a.MedicoNombre)
            .ThenBy(a => a.DiaSemana)
            .ThenBy(a => a.HoraInicio)
            .ToList();
    }

    public AgendaMedico? ObtenerPorId(int id)
    {
        return _agendas.FirstOrDefault(a => a.Id == id);
    }

    public bool MedicoAtiende(int medicoId, DayOfWeek dia, TimeOnly horaInicioTurno, int duracionMinutos = 30)
    {
        if (duracionMinutos <= 0) duracionMinutos = 30;
        TimeOnly horaFinTurno = horaInicioTurno.AddMinutes(duracionMinutos);

        return _agendas.Any(a =>
            a.Activo &&
            a.MedicoId == medicoId &&
            a.DiaSemana == dia &&
            horaInicioTurno >= a.HoraInicio &&
            horaFinTurno <= a.HoraFin);
    }

    /// <summary>
    /// Devuelve el rango horario de atencion (HoraInicio, HoraFin) que el admin
    /// configuro para el medico en el dia indicado. Si el medico NO atiende ese dia,
    /// devuelve null.
    /// Sirve para generar los slots correctos en el combo de Turnos.
    /// </summary>
    public (TimeOnly Hi, TimeOnly Hf)? ObtenerRangoAtencion(int medicoId, DayOfWeek dia)
    {
        var agenda = _agendas.FirstOrDefault(a =>
            a.Activo &&
            a.MedicoId == medicoId &&
            a.DiaSemana == dia);

        if (agenda == null)
            return null;

        return (agenda.HoraInicio, agenda.HoraFin);
    }

    public void Agregar(Medico medico, DayOfWeek dia, TimeOnly horaInicio, TimeOnly horaFin)
    {
        if (medico == null)
            throw new ArgumentException("Debe seleccionar un medico.");

        if (horaFin <= horaInicio)
            throw new ArgumentException("La hora de salida debe ser posterior a la hora de entrada.");

        var nueva = new AgendaMedico
        {
            MedicoId = medico.Id,
            Medico = medico,
            DiaSemana = dia,
            HoraInicio = horaInicio,
            HoraFin = horaFin
        };

        ValidarSuperposicion(nueva, null);

        nueva.Id = _siguienteId++;
        nueva.Activo = true;
        _agendas.Add(nueva);
    }

    public void Modificar(int agendaId, Medico medico, DayOfWeek dia, TimeOnly horaInicio, TimeOnly horaFin)
    {
        var existente = _agendas.FirstOrDefault(a => a.Id == agendaId)
            ?? throw new InvalidOperationException("El horario de agenda que intenta modificar no existe.");

        if (horaFin <= horaInicio)
            throw new ArgumentException("La hora de salida debe ser posterior a la hora de entrada.");

        var modificada = new AgendaMedico
        {
            Id = agendaId,
            MedicoId = medico.Id,
            Medico = medico,
            DiaSemana = dia,
            HoraInicio = horaInicio,
            HoraFin = horaFin
        };

        ValidarSuperposicion(modificada, agendaId);

        existente.MedicoId = medico.Id;
        existente.Medico = medico;
        existente.DiaSemana = dia;
        existente.HoraInicio = horaInicio;
        existente.HoraFin = horaFin;
    }

    public void EliminarLogico(int agendaId)
    {
        var existente = _agendas.FirstOrDefault(a => a.Id == agendaId)
            ?? throw new InvalidOperationException("Seleccione un horario.");

        existente.Activo = false;
    }

    private static void ValidarSuperposicion(AgendaMedico agenda, int? idAExcluir)
    {
        bool haySolape = _agendas.Any(a =>
            a.Activo &&
            a.Id != (idAExcluir ?? 0) &&
            a.MedicoId == agenda.MedicoId &&
            a.DiaSemana == agenda.DiaSemana &&
            a.HoraInicio < agenda.HoraFin &&
            agenda.HoraInicio < a.HoraFin);

        if (haySolape)
            throw new InvalidOperationException("El rango horario seleccionado se superpone con otro registro de agenda para el mismo medico y dia.");
    }
}
