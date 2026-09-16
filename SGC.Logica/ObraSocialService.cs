using SGC.Entidades;

namespace SGC.Logica;

public class ObraSocialService
{
    private static readonly List<ObraSocial> _obrasSociales = new()
    {
        new ObraSocial { Id = 1, Nombre = "OSDE", PorcentajeCobertura = 70, Activo = true },
        new ObraSocial { Id = 2, Nombre = "Swiss Medical", PorcentajeCobertura = 60, Activo = true },
        new ObraSocial { Id = 3, Nombre = "IOSCOR", PorcentajeCobertura = 50, Activo = true }
    };
    private static int _siguienteId = 4;

    public List<ObraSocial> ObtenerTodos()
    {
        return _obrasSociales.Where(o => o.Activo).ToList();
    }

    public ObraSocial? ObtenerPorId(int id)
    {
        return _obrasSociales.FirstOrDefault(o => o.Id == id);
    }

    public void Agregar(ObraSocial obraSocial)
    {
        Validar(obraSocial);

        if (_obrasSociales.Any(o => o.Activo && o.Nombre == obraSocial.Nombre))
            throw new InvalidOperationException($"Ya existe una obra social activa llamada {obraSocial.Nombre}.");

        obraSocial.Id = _siguienteId++;
        obraSocial.Activo = true;
        _obrasSociales.Add(obraSocial);
    }

    public void Modificar(ObraSocial obraSocial)
    {
        Validar(obraSocial);

        var existente = _obrasSociales.FirstOrDefault(o => o.Id == obraSocial.Id)
            ?? throw new InvalidOperationException("La obra social que intenta modificar no existe.");

        if (_obrasSociales.Any(o => o.Activo && o.Nombre == obraSocial.Nombre && o.Id != obraSocial.Id))
            throw new InvalidOperationException($"Ya existe otra obra social activa llamada {obraSocial.Nombre}.");

        existente.Nombre = obraSocial.Nombre;
        existente.PorcentajeCobertura = obraSocial.PorcentajeCobertura;
    }

    public void EliminarLogico(int id)
    {
        var obraSocial = _obrasSociales.FirstOrDefault(o => o.Id == id)
            ?? throw new InvalidOperationException("La obra social que intenta eliminar no existe.");

        obraSocial.Activo = false;
    }

    private void Validar(ObraSocial obraSocial)
    {
        if (string.IsNullOrWhiteSpace(obraSocial.Nombre))
            throw new ArgumentException("El nombre de la obra social es obligatorio.");

        if (obraSocial.PorcentajeCobertura < 0 || obraSocial.PorcentajeCobertura > 100)
            throw new ArgumentException("El porcentaje de cobertura debe estar entre 0 y 100.");
    }
}
