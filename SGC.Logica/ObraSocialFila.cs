using SGC.Entidades;

namespace SGC.Logica;

// Fila del listado de obras sociales: la obra social mas datos calculados.
// Solo informacion administrativa, nunca datos clinicos (RNF#07).
public class ObraSocialFila
{
    public required ObraSocial ObraSocial { get; init; }

    // Medicos activos que la aceptan (apellido, nombre), ordenados.
    public IReadOnlyList<string> Medicos { get; init; } = Array.Empty<string>();

    // Pacientes activos afiliados a esta obra social.
    public int PacientesAfiliados { get; init; }

    public int Id => ObraSocial.Id;
    public string Nombre => ObraSocial.Nombre;
    public decimal PorcentajeCobertura => ObraSocial.PorcentajeCobertura;
    public int MedicosQueAceptan => Medicos.Count;
}
