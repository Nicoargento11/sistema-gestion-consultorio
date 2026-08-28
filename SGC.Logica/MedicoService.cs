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

    public List<Medico> ObtenerTodos()
    {
        return _medicos.Where(m => m.Activo).ToList();
    }

    public Medico? ObtenerPorId(int id)
    {
        return _medicos.FirstOrDefault(m => m.Id == id);
    }
}
