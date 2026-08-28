using SGC.Entidades;

namespace SGC.Logica;

public class HorarioService
{
    // TODO (companero): reemplazar por la configuracion real de agenda por medico.
    private static readonly List<Horario> _horarios = new()
    {
        new Horario { Id = 1, HoraInicio = new TimeOnly(8, 0), HoraFin = new TimeOnly(8, 30), Activo = true },
        new Horario { Id = 2, HoraInicio = new TimeOnly(8, 30), HoraFin = new TimeOnly(9, 0), Activo = true },
        new Horario { Id = 3, HoraInicio = new TimeOnly(9, 0), HoraFin = new TimeOnly(9, 30), Activo = true },
        new Horario { Id = 4, HoraInicio = new TimeOnly(9, 30), HoraFin = new TimeOnly(10, 0), Activo = true },
        new Horario { Id = 5, HoraInicio = new TimeOnly(10, 0), HoraFin = new TimeOnly(10, 30), Activo = true },
        new Horario { Id = 6, HoraInicio = new TimeOnly(10, 30), HoraFin = new TimeOnly(11, 0), Activo = true }
    };

    public List<Horario> ObtenerTodos()
    {
        return _horarios.Where(h => h.Activo).ToList();
    }

    public Horario? ObtenerPorId(int id)
    {
        return _horarios.FirstOrDefault(h => h.Id == id);
    }
}
