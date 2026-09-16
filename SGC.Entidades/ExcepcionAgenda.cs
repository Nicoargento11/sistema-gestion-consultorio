namespace SGC.Entidades;

public enum TipoExcepcionAgenda
{
    DiaCompleto,
    RangoHorario
}

public class ExcepcionAgenda
{
    public int Id { get; set; }

    public int MedicoId { get; set; }
    public Medico? Medico { get; set; }

    public DateOnly Fecha { get; set; }
    public TipoExcepcionAgenda Tipo { get; set; }
    public TimeOnly? HoraInicio { get; set; }
    public TimeOnly? HoraFin { get; set; }
    public string Motivo { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;

    public string MedicoNombre => Medico?.NombreCompleto ?? "";

    public string DescripcionTexto => Tipo == TipoExcepcionAgenda.DiaCompleto
        ? "Dia completo"
        : $"{HoraInicio:HH:mm} - {HoraFin:HH:mm}";
}
