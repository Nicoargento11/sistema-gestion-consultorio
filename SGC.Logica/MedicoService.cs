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

    private readonly ObraSocialService _obraSocialService = new();

    public List<Medico> ObtenerTodos()
    {
        return _medicos.Where(m => m.Activo).Select(ResolverNavegacion).ToList();
    }

    public Medico? ObtenerPorId(int id)
    {
        var medico = _medicos.FirstOrDefault(m => m.Id == id);
        return medico is null ? null : ResolverNavegacion(medico);
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
        existente.PrecioConsultaParticular = medico.PrecioConsultaParticular;
        existente.ObrasSocialesAceptadasIds = medico.ObrasSocialesAceptadasIds;
    }

    public void EliminarLogico(int id)
    {
        var medico = _medicos.FirstOrDefault(m => m.Id == id)
            ?? throw new InvalidOperationException("El medico que intenta eliminar no existe.");

        medico.Activo = false;
    }

    private Medico ResolverNavegacion(Medico medico)
    {
        medico.ObrasSocialesAceptadas = medico.ObrasSocialesAceptadasIds
            .Select(id => _obraSocialService.ObtenerPorId(id))
            .Where(o => o is not null)
            .Cast<ObraSocial>()
            .ToList();
        return medico;
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

        if (medico.PrecioConsultaParticular < 0)
            throw new ArgumentException("El precio de consulta particular no puede ser negativo.");

        foreach (var obraSocialId in medico.ObrasSocialesAceptadasIds)
        {
            var obraSocial = _obraSocialService.ObtenerPorId(obraSocialId);
            if (obraSocial is null || !obraSocial.Activo)
                throw new ArgumentException("Selecciono una obra social que no es valida.");
        }
    }
}
