namespace SGC.Entidades;

public class AgendaMedico
{
    public int Id { get; set; }

    public int MedicoId { get; set; }
    public Medico? Medico { get; set; }

    public TimeOnly HoraInicio { get; set; }
    public TimeOnly HoraFin { get; set; }

    public DayOfWeek DiaSemana { get; set; }
    public bool Activo { get; set; } = true;

    public string DiaNombre => NombreDia(DiaSemana);

    public string HorarioRango => $"{HoraInicio:HH:mm} - {HoraFin:HH:mm}";
    public string MedicoNombre => Medico?.NombreCompleto ?? "";

    public static string NombreDia(DayOfWeek dia)
    {
        return dia switch
        {
            DayOfWeek.Monday => "Lunes",
            DayOfWeek.Tuesday => "Martes",
            DayOfWeek.Wednesday => "Miercoles",
            DayOfWeek.Thursday => "Jueves",
            DayOfWeek.Friday => "Viernes",
            DayOfWeek.Saturday => "Sabado",
            DayOfWeek.Sunday => "Domingo",
            _ => dia.ToString()
        };
    }
}