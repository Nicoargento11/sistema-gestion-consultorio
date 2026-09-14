using SGC.Entidades;

namespace SGC.Logica;

public class HorarioService
{
    private static readonly List<Horario> _horarios = new()
    {
        new Horario { Id = 1, HoraInicio = new TimeOnly(8, 0), HoraFin = new TimeOnly(8, 30), Activo = true },
        new Horario { Id = 2, HoraInicio = new TimeOnly(8, 30), HoraFin = new TimeOnly(9, 0), Activo = true },
        new Horario { Id = 3, HoraInicio = new TimeOnly(9, 0), HoraFin = new TimeOnly(9, 30), Activo = true },
        new Horario { Id = 4, HoraInicio = new TimeOnly(9, 30), HoraFin = new TimeOnly(10, 0), Activo = true },
        new Horario { Id = 5, HoraInicio = new TimeOnly(10, 0), HoraFin = new TimeOnly(10, 30), Activo = true },
        new Horario { Id = 6, HoraInicio = new TimeOnly(10, 30), HoraFin = new TimeOnly(11, 0), Activo = true }
    };
    private static int _siguienteId = 7;

    public List<Horario> ObtenerTodos()
    {
        return _horarios.Where(h => h.Activo).OrderBy(h => h.HoraInicio).ToList();
    }

    public Horario? ObtenerPorId(int id)
    {
        return _horarios.FirstOrDefault(h => h.Id == id);
    }

    public Horario ObtenerOCrear(TimeOnly horaInicio, TimeOnly horaFin)
    {
        ValidarRango(horaInicio, horaFin);

        var existente = _horarios.FirstOrDefault(h =>
            h.Activo && h.HoraInicio == horaInicio && h.HoraFin == horaFin);

        if (existente != null)
            return existente;

        var nuevo = new Horario
        {
            Id = _siguienteId++,
            HoraInicio = horaInicio,
            HoraFin = horaFin,
            Activo = true
        };
        _horarios.Add(nuevo);
        return nuevo;
    }

    public void Modificar(int id, TimeOnly horaInicio, TimeOnly horaFin)
    {
        ValidarRango(horaInicio, horaFin);

        var horario = _horarios.FirstOrDefault(h => h.Id == id)
            ?? throw new InvalidOperationException("El horario que intenta modificar no existe.");

        horario.HoraInicio = horaInicio;
        horario.HoraFin = horaFin;
    }

    public void EliminarLogico(int id)
    {
        var horario = _horarios.FirstOrDefault(h => h.Id == id)
            ?? throw new InvalidOperationException("El horario que intenta eliminar no existe.");

        horario.Activo = false;
    }

    private static void ValidarRango(TimeOnly horaInicio, TimeOnly horaFin)
    {
        if (horaFin <= horaInicio)
            throw new ArgumentException("La hora de salida debe ser posterior a la hora de entrada.");
    }
}
