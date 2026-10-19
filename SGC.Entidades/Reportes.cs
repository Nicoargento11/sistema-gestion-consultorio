namespace SGC.Entidades;

public class ReporteMensualItem
{
    public int MesNumero { get; set; }
    public string NombreMes { get; set; } = string.Empty;
    public int TotalTurnos { get; set; }
    public int PacientesAtendidos { get; set; }
    public int Ausentes { get; set; }
    public int Cancelados { get; set; }
    public int PendientesOConfirmados { get; set; }
}

public class ReporteObraSocialItem
{
    public string ObraSocial { get; set; } = string.Empty;
    public int CantidadTurnos { get; set; }
    public decimal TotalRecaudado { get; set; }
}

public class ReporteMedicoRendimientoItem
{
    public int MedicoId { get; set; }
    public string MedicoNombre { get; set; } = string.Empty;
    public string Especialidad { get; set; } = string.Empty;
    public int TotalTurnos { get; set; }
    public int TurnosAtendidos { get; set; }
    public int TurnosAusentes { get; set; }
    public int TurnosCancelados { get; set; }
    public decimal TotalRecaudado { get; set; }
}
