using Microsoft.EntityFrameworkCore;
using SGC.Datos;
using SGC.Entidades;

namespace SGC.Logica;

public class MedicoService
{
    public List<Medico> ObtenerTodos()
    {
        using var contexto = SGCContextFactory.Crear();
        return contexto.Medicos
            .Where(m => m.Activo)
            .Include(m => m.ObrasSocialesAceptadas)
            .ToList();
    }

    public Medico? ObtenerPorId(int id)
    {
        using var contexto = SGCContextFactory.Crear();
        return contexto.Medicos
            .Include(m => m.ObrasSocialesAceptadas)
            .FirstOrDefault(m => m.Id == id);
    }

    public void Agregar(Medico medico)
    {
        using var contexto = SGCContextFactory.Crear();
        Validar(medico, contexto);

        medico.Id = 0;
        medico.Activo = true;
        medico.ObrasSocialesAceptadas = contexto.ObrasSociales
            .Where(o => medico.ObrasSocialesAceptadasIds.Contains(o.Id))
            .ToList();

        contexto.Medicos.Add(medico);
        contexto.SaveChanges();
    }

    public void Modificar(Medico medico)
    {
        using var contexto = SGCContextFactory.Crear();
        Validar(medico, contexto);

        var existente = contexto.Medicos
            .Include(m => m.ObrasSocialesAceptadas)
            .FirstOrDefault(m => m.Id == medico.Id)
            ?? throw new InvalidOperationException("El medico que intenta modificar no existe.");

        existente.Nombre = medico.Nombre;
        existente.Apellido = medico.Apellido;
        existente.Dni = medico.Dni;
        existente.Matricula = medico.Matricula;
        existente.Especialidad = medico.Especialidad;
        existente.PrecioConsultaParticular = medico.PrecioConsultaParticular;

        existente.ObrasSocialesAceptadas = contexto.ObrasSociales
            .Where(o => medico.ObrasSocialesAceptadasIds.Contains(o.Id))
            .ToList();

        contexto.SaveChanges();
    }

    public void EliminarLogico(int id)
    {
        using var contexto = SGCContextFactory.Crear();
        var medico = contexto.Medicos.FirstOrDefault(m => m.Id == id)
            ?? throw new InvalidOperationException("El medico que intenta eliminar no existe.");

        medico.Activo = false;
        contexto.SaveChanges();
    }

    private void Validar(Medico medico, SGCContext contexto)
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

        if (contexto.Medicos.Any(m => m.Activo && m.Dni == medico.Dni && m.Id != medico.Id))
            throw new InvalidOperationException($"Ya existe un medico activo con el DNI {medico.Dni}.");

        foreach (var obraSocialId in medico.ObrasSocialesAceptadasIds)
        {
            var obraSocial = contexto.ObrasSociales.FirstOrDefault(o => o.Id == obraSocialId);
            if (obraSocial is null || !obraSocial.Activo)
                throw new ArgumentException("Selecciono una obra social que no es valida.");
        }
    }
}
