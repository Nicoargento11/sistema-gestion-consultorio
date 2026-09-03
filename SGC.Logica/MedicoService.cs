using SGC.Entidades;

namespace SGC.Logica;

public class MedicoService
{
    // TODO (companero): reemplazar por ABM real cuando este listo SGC.Datos.
    private static readonly List<Medico> _medicos = new()
    {
        new Medico { Id = 1, Dni = "20111222", Nombre = "Laura", Apellido = "Gomez", Matricula = "MP1234", Especialidad = "Clinica General", Activo = true },
        new Medico { Id = 2, Dni = "20333444", Nombre = "Juan", Apellido = "Perez", Matricula = "MP5678", Especialidad = "Cardiologia", Activo = true },
        new Medico { Id = 3, Dni = "20555666", Nombre = "Maria", Apellido = "Fernandez", Matricula = "MP9012", Especialidad = "Pediatria", Activo = true }
    };
    private static int _siguienteId = 4;

    public List<Medico> ObtenerTodos()
    {
        return _medicos.Where(m => m.Activo).ToList();
    }

    public Medico? ObtenerPorId(int id)
    {
        return _medicos.FirstOrDefault(m => m.Id == id);
    }

    public void Agregar(Medico medico)
    {
        Validar(medico);

        if (_medicos.Any(m => m.Activo && m.Dni == medico.Dni))
            throw new InvalidOperationException($"Ya existe un medico activo con el DNI {medico.Dni}.");

        medico.Id = _siguienteId++;
        medico.Activo = true;
        _medicos.Add(medico);
    }

    public void Modificar(Medico medico)
    {
        Validar(medico);

        var existente = _medicos.FirstOrDefault(m => m.Id == medico.Id)
            ?? throw new InvalidOperationException("El medico que intenta modificar no existe.");

        if (_medicos.Any(m => m.Activo && m.Dni == medico.Dni && m.Id != medico.Id))
            throw new InvalidOperationException($"Ya existe otro medico activo con el DNI {medico.Dni}.");

        existente.Nombre = medico.Nombre;
        existente.Apellido = medico.Apellido;
        existente.Dni = medico.Dni;
        existente.Matricula = medico.Matricula;
        existente.Especialidad = medico.Especialidad;
    }

    public void EliminarLogico(int id)
    {
        var medico = _medicos.FirstOrDefault(m => m.Id == id)
            ?? throw new InvalidOperationException("El medico que intenta eliminar no existe.");

        medico.Activo = false;
    }

    private void Validar(Medico medico)
    {
        if (string.IsNullOrWhiteSpace(medico.Nombre))
            throw new ArgumentException("El nombre es obligatorio.");

        if (string.IsNullOrWhiteSpace(medico.Apellido))
            throw new ArgumentException("El apellido es obligatorio.");

        if (string.IsNullOrWhiteSpace(medico.Dni) || !medico.Dni.All(char.IsDigit) ||
            medico.Dni.Length < 7 || medico.Dni.Length > 8)
            throw new ArgumentException("El DNI debe tener entre 7 y 8 digitos numericos, sin puntos ni letras.");

        if (string.IsNullOrWhiteSpace(medico.Matricula))
            throw new ArgumentException("La matricula es obligatoria.");

        if (string.IsNullOrWhiteSpace(medico.Especialidad))
            throw new ArgumentException("La especialidad es obligatoria.");
    }
}
