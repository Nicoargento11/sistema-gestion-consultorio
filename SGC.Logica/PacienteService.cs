using SGC.Entidades;

namespace SGC.Logica;

public class PacienteService
{
    // Datos iniciales de prueba para desarrollo y demostracion
    private static readonly List<Paciente> _pacientes = new()
    {
        new Paciente { Id = 1, Nombre = "Carlos", Apellido = "Fernandez", Dni = "35123456", Email = "carlos.f@email.com", Telefono = "3794123456", Activo = true },
        new Paciente { Id = 2, Nombre = "Ana", Apellido = "Martinez", Dni = "38987654", Email = "ana.martinez@email.com", Telefono = "3794987654", Activo = true },
        new Paciente { Id = 3, Nombre = "Luis", Apellido = "Torres", Dni = "40555666", Email = "luis.torres@email.com", Telefono = "3794555666", Activo = true },
        new Paciente { Id = 4, Nombre = "Sofia", Apellido = "Herrera", Dni = "42111222", Email = "sofia.herrera@email.com", Telefono = "3794111222", Activo = true }
    };
    private static int _siguienteId = 5;

    public List<Paciente> ObtenerTodos()
    {
        return _pacientes.Where(p => p.Activo).ToList();
    }

    public void Agregar(Paciente paciente)
    {
        paciente.ObraSocial = paciente.ObraSocial.Trim();
        Validar(paciente);

        if (_pacientes.Any(p => p.Activo && p.Dni == paciente.Dni))
            throw new InvalidOperationException($"Ya existe un paciente activo con el DNI {paciente.Dni}.");

        paciente.Id = _siguienteId++;
        paciente.Activo = true;
        _pacientes.Add(paciente);
    }

    public void Modificar(Paciente paciente)
    {
        paciente.ObraSocial = paciente.ObraSocial.Trim();
        Validar(paciente);

        var existente = _pacientes.FirstOrDefault(p => p.Id == paciente.Id)
            ?? throw new InvalidOperationException("El paciente que intenta modificar no existe.");

        if (_pacientes.Any(p => p.Activo && p.Dni == paciente.Dni && p.Id != paciente.Id))
            throw new InvalidOperationException($"Ya existe otro paciente activo con el DNI {paciente.Dni}.");

        existente.Nombre = paciente.Nombre;
        existente.Apellido = paciente.Apellido;
        existente.Dni = paciente.Dni;
        existente.Email = paciente.Email;
        existente.Telefono = paciente.Telefono;
        existente.FechaNacimiento = paciente.FechaNacimiento;
        existente.ObraSocial = paciente.ObraSocial;
    }

    public void EliminarLogico(int id)
    {
        var paciente = _pacientes.FirstOrDefault(p => p.Id == id)
            ?? throw new InvalidOperationException("El paciente que intenta eliminar no existe.");

        paciente.Activo = false;
    }

    private void Validar(Paciente paciente)
    {
        if (string.IsNullOrWhiteSpace(paciente.Nombre))
            throw new ArgumentException("El nombre es obligatorio.");

        if (string.IsNullOrWhiteSpace(paciente.Apellido))
            throw new ArgumentException("El apellido es obligatorio.");

        if (string.IsNullOrWhiteSpace(paciente.Dni) || !paciente.Dni.All(char.IsDigit) ||
            paciente.Dni.Length < 7 || paciente.Dni.Length > 8)
            throw new ArgumentException("El DNI debe tener entre 7 y 8 dígitos numéricos, sin puntos ni letras.");

        if (string.IsNullOrWhiteSpace(paciente.Email) || !paciente.Email.Contains('@') || !paciente.Email.Contains('.'))
            throw new ArgumentException("Debe ingresar un email con formato válido (ej: nombre@dominio.com).");

        if (string.IsNullOrWhiteSpace(paciente.Telefono))
            throw new ArgumentException("El teléfono es obligatorio.");

        if (paciente.FechaNacimiento == default)
            throw new ArgumentException("Debe ingresar la fecha de nacimiento.");

        if (paciente.FechaNacimiento > DateOnly.FromDateTime(DateTime.Today))
            throw new ArgumentException("La fecha de nacimiento no puede ser futura.");

        if (paciente.FechaNacimiento < DateOnly.FromDateTime(DateTime.Today).AddYears(-120))
            throw new ArgumentException("La fecha de nacimiento no es válida.");

        // ObraSocial es opcional: vacío se interpreta como "Particular".
    }
}
