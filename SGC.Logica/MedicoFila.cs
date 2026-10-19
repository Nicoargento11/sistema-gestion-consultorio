using SGC.Entidades;

namespace SGC.Logica;

// Fila del listado de medicos: el medico mas datos calculados. Solo
// informacion administrativa, nunca datos clinicos (RNF#07).
public class MedicoFila
{
    public required Medico Medico { get; init; }

    // Nombres de las obras sociales activas que acepta, ordenados.
    public IReadOnlyList<string> ObrasSociales { get; init; } = Array.Empty<string>();

    // Turnos activos no cancelados del mes calendario actual.
    public int TurnosDelMes { get; init; }

    // Turnos activos no cancelados de hoy.
    public int TurnosDeHoy { get; init; }

    public int Id => Medico.Id;
    public string Nombre => Medico.Nombre;
    public string Apellido => Medico.Apellido;
    public string Dni => Medico.Dni;
    public string Matricula => Medico.Matricula;
    public string Especialidad => Medico.Especialidad;
    public decimal PrecioConsultaParticular => Medico.PrecioConsultaParticular;
    public string ObrasSocialesTexto => ObrasSociales.Count == 0 ? "-" : string.Join(", ", ObrasSociales);
}
