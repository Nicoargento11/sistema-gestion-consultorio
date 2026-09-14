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

        var horarioService = new HorarioService();
        var medicoService = new MedicoService();
        var horarios = horarioService.ObtenerTodos();
        var medicos = medicoService.ObtenerTodos();

        var diasHabiles = new[]
        {
            DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday
        };

        foreach (var medico in medicos)
        {
            foreach (var dia in diasHabiles)
            {
                foreach (var horario in horarios)
                {
                    _agendas.Add(new AgendaMedico
                    {
                        Id = _siguienteId++,
                        MedicoId = medico.Id,
                        Medico = medico,
                        HorarioId = horario.Id,
                        Horario = horario,
                        DiaSemana = dia,
                        Activo = true
                    });
                }
            }
        }

        _semillaCargada = true;
    }

    public List<AgendaMedico> Consultar(int? medicoId = null, DayOfWeek? dia = null, int? horarioId = null)
    {
        IEnumerable<AgendaMedico> query = _agendas.Where(a => a.Activo);

        if (medicoId.HasValue)
            query = query.Where(a => a.MedicoId == medicoId.Value);

        if (dia.HasValue)
            query = query.Where(a => a.DiaSemana == dia.Value);

        if (horarioId.HasValue)
            query = query.Where(a => a.HorarioId == horarioId.Value);

        return query
            .OrderBy(a => a.MedicoNombre)
            .ThenBy(a => a.DiaSemana)
            .ThenBy(a => a.Horario != null ? a.Horario.HoraInicio : TimeOnly.MinValue)
            .ToList();
    }

    public AgendaMedico? ObtenerPorId(int id)
    {
        return _agendas.FirstOrDefault(a => a.Id == id);
    }

    public bool MedicoAtiende(int medicoId, DayOfWeek dia, int horarioId)
    {
        return _agendas.Any(a =>
            a.Activo &&
            a.MedicoId == medicoId &&
            a.DiaSemana == dia &&
            a.HorarioId == horarioId);
    }

    public void Agregar(Medico medico, DayOfWeek dia, Horario horario)
    {
        if (medico == null)
            throw new ArgumentException("Debe seleccionar un medico.");

        if (horario == null)
            throw new ArgumentException("Debe completar hora de entrada y hora de salida.");

        if (_agendas.Any(a => a.Activo && a.MedicoId == medico.Id && a.DiaSemana == dia && a.HorarioId == horario.Id))
            throw new InvalidOperationException("Ese medico ya tiene ese bloque horario en el dia seleccionado.");

        _agendas.Add(new AgendaMedico
        {
            Id = _siguienteId++,
            MedicoId = medico.Id,
            Medico = medico,
            HorarioId = horario.Id,
            Horario = horario,
            DiaSemana = dia,
            Activo = true
        });
    }

    public void Modificar(int agendaId, Medico medico, DayOfWeek dia, Horario horario)
    {
        var existente = _agendas.FirstOrDefault(a => a.Id == agendaId)
            ?? throw new InvalidOperationException("El horario de agenda que intenta modificar no existe.");

        if (_agendas.Any(a =>
            a.Activo &&
            a.Id != agendaId &&
            a.MedicoId == medico.Id &&
            a.DiaSemana == dia &&
            a.HorarioId == horario.Id))
        {
            throw new InvalidOperationException("Ese medico ya tiene ese bloque horario en el dia seleccionado.");
        }

        existente.MedicoId = medico.Id;
        existente.Medico = medico;
        existente.DiaSemana = dia;
        existente.HorarioId = horario.Id;
        existente.Horario = horario;
    }

    public void EliminarLogico(int agendaId)
    {
        var existente = _agendas.FirstOrDefault(a => a.Id == agendaId)
            ?? throw new InvalidOperationException("Seleccione un horario.");

        existente.Activo = false;
    }
}
