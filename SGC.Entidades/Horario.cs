namespace SGC.Entidades;

public class Horario
{
    public int Id { get; set; }
    public TimeOnly HoraInicio { get; set; }
    public TimeOnly HoraFin { get; set; }
    public bool Activo { get; set; } = true;
}
