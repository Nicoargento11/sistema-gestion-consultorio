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

    /// <summary>
    /// Genera slots horarios consecutivos DENTRO de un rango de atencion del medico.
    /// Cada slot dura "duracionSlotMin" minutos y avanza de "saltoMin" en "saltoMin".
    /// Los slots se persisten automaticamente (ObtenerOCrear) para que tengan Id y
    /// se puedan bindear al ComboBox de Turnos.
    /// Ej: rango 06:00-09:00, duracion 45min, salto 45min → 06:00-06:45, 06:45-07:30, 07:30-08:15, 08:15-09:00
    /// Ej: rango 09:00-11:00, duracion 120min, salto 30min → 09:00-11:00
    /// </summary>
    public List<Horario> GenerarSlotsEnRango(
        TimeOnly rangoInicio,
        TimeOnly rangoFin,
        int duracionSlotMin = 30,
        int saltoMin = 30)
    {
        if (duracionSlotMin <= 0) duracionSlotMin = 30;
        if (saltoMin <= 0) saltoMin = 30;

        var resultado = new List<Horario>();

        // Si el rango es invalido (fin <= inicio) no generamos nada
        if (rangoFin <= rangoInicio)
            return resultado;

        // Vamos avanzando desde rangoInicio, agregando slots que entren completos
        TimeOnly actual = rangoInicio;
        while (actual.AddMinutes(duracionSlotMin) <= rangoFin)
        {
            TimeOnly finSlot = actual.AddMinutes(duracionSlotMin);
            resultado.Add(ObtenerOCrear(actual, finSlot));
            actual = actual.AddMinutes(saltoMin);
        }

        return resultado;
    }

    private static void ValidarRango(TimeOnly horaInicio, TimeOnly horaFin)
    {
        if (horaFin <= horaInicio)
            throw new ArgumentException("La hora de salida debe ser posterior a la hora de entrada.");
    }
}
