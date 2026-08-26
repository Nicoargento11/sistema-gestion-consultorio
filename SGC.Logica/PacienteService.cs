using SGC.Entidades;

namespace SGC.Logica;

public class PacienteService
{
    // TODO: reemplazar por SGCContext.Pacientes cuando conectemos la base de datos real.
    private static readonly List<Paciente> _pacientes = new();
    private static int _siguienteId = 1;

    public List<Paciente> ObtenerTodos()
    {
        return _pacientes.Where(p => p.Activo).ToList();
    }

    public void Agregar(Paciente paciente)
    {
        Validar(paciente);

        if (_pacientes.Any(p => p.Activo && p.Dni == paciente.Dni))
            throw new InvalidOperationException($"Ya existe un paciente activo con el DNI {paciente.Dni}.");

        paciente.Id = _siguienteId++;
        paciente.Activo = true;
        _pacientes.Add(paciente);
    }

    public void Modificar(Paciente paciente)
    {
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
    }
}
