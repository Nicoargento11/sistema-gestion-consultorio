using SGC.Entidades;

namespace SGC.Logica;

// Fila del listado de pacientes: el paciente mas datos calculados a partir de
// sus turnos. Solo informacion administrativa, nunca datos clinicos (RNF#07).
public class PacienteFila
{
    public required Paciente Paciente { get; init; }

    // Fecha del ultimo turno con Estado Asistio (null = nunca asistio).
    public DateOnly? UltimaVisita { get; init; }

    // Texto "dd/MM/yyyy HH:mm - Medico" del proximo turno futuro Pendiente o
    // Confirmado, o "-" si no tiene (o si ninguno es de un medico visible).
    public string ProximoTurno { get; init; } = "-";

    // Cantidad de turnos con Estado Ausente.
    public int Ausencias { get; init; }

    public int Id => Paciente.Id;
    public string Nombre => Paciente.Nombre;
    public string Apellido => Paciente.Apellido;
    public DateOnly FechaNacimiento => Paciente.FechaNacimiento;
    public string Dni => Paciente.Dni;
    public string Email => Paciente.Email;
    public string Telefono => Paciente.Telefono;
    public string ObraSocialNombre => Paciente.ObraSocialNombre;
    public int? Edad => Paciente.Edad;
}
