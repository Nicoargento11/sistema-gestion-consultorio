namespace SGC.Entidades;

public class AgendaMedico
{
    public int Id { get; set; }

    public int MedicoId { get; set; }
    public Medico? Medico { get; set; }

    public int HorarioId { get; set; }
    public Horario? Horario { get; set; }

    public DayOfWeek DiaSemana { get; set; }
    public bool Activo { get; set; } = true;
}
