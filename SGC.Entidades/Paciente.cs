namespace SGC.Entidades;

public class Paciente
{
    public int Id { get; set; }
    public string Dni { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public DateOnly FechaNacimiento { get; set; }
    public bool Activo { get; set; } = true;

    // null = Particular (sin obra social).
    public int? ObraSocialId { get; set; }
    public ObraSocial? ObraSocial { get; set; }

    public string NombreCompleto => $"{Apellido}, {Nombre}";
    public string ObraSocialNombre => ObraSocial?.Nombre ?? "Particular";

    // Edad en anios cumplidos hoy; null si la fecha de nacimiento no es valida.
    public int? Edad => CalcularEdad(FechaNacimiento, DateOnly.FromDateTime(DateTime.Today));

    // Anios cumplidos al dia "referencia". Resta uno si todavia no llego el
    // cumpleanos de ese anio (AddYears lleva el 29/02 al 28/02 en anios no
    // bisiestos). Devuelve null para una fecha sin cargar o posterior.
    public static int? CalcularEdad(DateOnly nacimiento, DateOnly referencia)
    {
        if (nacimiento == default || nacimiento > referencia)
            return null;

        int edad = referencia.Year - nacimiento.Year;
        if (referencia < nacimiento.AddYears(edad))
            edad--;
        return edad;
    }

    public ICollection<Turno> Turnos { get; set; } = new List<Turno>();
}
