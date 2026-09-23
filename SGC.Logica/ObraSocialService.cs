using SGC.Datos;
using SGC.Entidades;

namespace SGC.Logica;

public class ObraSocialService
{
    public List<ObraSocial> ObtenerTodos()
    {
        using var contexto = SGCContextFactory.Crear();
        return contexto.ObrasSociales.Where(o => o.Activo).ToList();
    }

    public ObraSocial? ObtenerPorId(int id)
    {
        using var contexto = SGCContextFactory.Crear();
        return contexto.ObrasSociales.FirstOrDefault(o => o.Id == id);
    }

    public void Agregar(ObraSocial obraSocial)
    {
        using var contexto = SGCContextFactory.Crear();
        Validar(obraSocial, contexto);

        obraSocial.Id = 0;
        obraSocial.Activo = true;
        contexto.ObrasSociales.Add(obraSocial);
        contexto.SaveChanges();
    }

    public void Modificar(ObraSocial obraSocial)
    {
        using var contexto = SGCContextFactory.Crear();
        Validar(obraSocial, contexto);

        var existente = contexto.ObrasSociales.FirstOrDefault(o => o.Id == obraSocial.Id)
            ?? throw new InvalidOperationException("La obra social que intenta modificar no existe.");

        existente.Nombre = obraSocial.Nombre;
        existente.PorcentajeCobertura = obraSocial.PorcentajeCobertura;
        contexto.SaveChanges();
    }

    public void EliminarLogico(int id)
    {
        using var contexto = SGCContextFactory.Crear();
        var obraSocial = contexto.ObrasSociales.FirstOrDefault(o => o.Id == id)
            ?? throw new InvalidOperationException("La obra social que intenta eliminar no existe.");

        obraSocial.Activo = false;
        contexto.SaveChanges();
    }

    private void Validar(ObraSocial obraSocial, SGCContext contexto)
    {
        if (string.IsNullOrWhiteSpace(obraSocial.Nombre))
            throw new ArgumentException("El nombre de la obra social es obligatorio.");

        if (obraSocial.PorcentajeCobertura < 0 || obraSocial.PorcentajeCobertura > 100)
            throw new ArgumentException("El porcentaje de cobertura debe estar entre 0 y 100.");

        if (contexto.ObrasSociales.Any(o => o.Activo && o.Nombre == obraSocial.Nombre && o.Id != obraSocial.Id))
            throw new InvalidOperationException($"Ya existe otra obra social activa llamada {obraSocial.Nombre}.");
    }
}
