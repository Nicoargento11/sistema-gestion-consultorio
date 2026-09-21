using SGC.Entidades;

namespace SGC.Logica;

public class AgendaMedicoService
{
    private static readonly List<AgendaMedico> _agenda = new();
    private static int _siguienteId = 1;

    private readonly MedicoService _medicoService = new();

    public List<AgendaMedico> ObtenerTodos()
    {
        return _agenda.Where(a => a.Activo).ToList();
    }

    public List<AgendaMedico> ObtenerPorMedico(int medicoId)
    {
        return _agenda.Where(a => a.Activo && a.MedicoId == medicoId).ToList();
    }

    public AgendaMedico? ObtenerPorId(int id)
    {
        return _agenda.FirstOrDefault(a => a.Id == id);
    }

    public List<AgendaMedico> ObtenerPorMedicoYDia(int medicoId, DayOfWeek dia)
    {
        return _agenda.Where(a => a.Activo && a.MedicoId == medicoId && a.DiaSemana == dia).ToList();
    }

    public bool MedicoAtiende(int medicoId, DayOfWeek dia, TimeOnly horaInicioTurno, int duracionMinutos)
    {
        if (duracionMinutos <= 0) duracionMinutos = 30;
        TimeOnly horaFinTurno = horaInicioTurno.AddMinutes(duracionMinutos);

        return ObtenerPorMedicoYDia(medicoId, dia).Any(a =>
            horaInicioTurno >= a.HoraInicio &&
            horaFinTurno <= a.HoraFin);
    }

    public void Agregar(AgendaMedico agenda)
    {
        var medico = ValidarDatosBasicos(agenda);

        ValidarSuperposicion(agenda, idAExcluir: null);

        agenda.Id = _siguienteId++;
        agenda.Activo = true;
        agenda.Medico = medico;
        _agenda.Add(agenda);
    }

    public void Modificar(AgendaMedico agenda)
    {
        var medico = ValidarDatosBasicos(agenda);

        var existente = _agenda.FirstOrDefault(a => a.Id == agenda.Id)
            ?? throw new InvalidOperationException("El registro de agenda que intenta modificar no existe.");

        ValidarSuperposicion(agenda, idAExcluir: existente.Id);

        existente.MedicoId = agenda.MedicoId;
        existente.Medico = medico;
        existente.DiaSemana = agenda.DiaSemana;
        existente.HoraInicio = agenda.HoraInicio;
        existente.HoraFin = agenda.HoraFin;
    }

    public void EliminarLogico(int id)
    {
        var agenda = _agenda.FirstOrDefault(a => a.Id == id)
            ?? throw new InvalidOperationException("El registro de agenda que intenta eliminar no existe.");

        agenda.Activo = false;
    }

    private void ValidarSuperposicion(AgendaMedico agenda, int? idAExcluir)
    {
        var franjasDelDia = ObtenerPorMedico(agenda.MedicoId)
            .Where(a => a.DiaSemana == agenda.DiaSemana && a.Id != idAExcluir);

        foreach (var a in franjasDelDia)
        {
            if (a.HoraInicio < agenda.HoraFin && agenda.HoraInicio < a.HoraFin)
            {
                throw new InvalidOperationException("El rango horario seleccionado se superpone con otro registro de agenda para el mismo medico y dia de semana.");
            }
        }
    }

    private Medico ValidarDatosBasicos(AgendaMedico agenda)
    {
        var medico = _medicoService.ObtenerPorId(agenda.MedicoId);
        if (medico is null || !medico.Activo)
            throw new ArgumentException("Debe seleccionar un medico valido.");

        if (agenda.HoraFin <= agenda.HoraInicio)
            throw new ArgumentException("La hora de fin debe ser posterior a la hora de inicio.");

        return medico;
    }
}
