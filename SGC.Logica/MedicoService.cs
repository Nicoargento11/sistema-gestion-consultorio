using Microsoft.EntityFrameworkCore;
using SGC.Datos;
using SGC.Entidades;

namespace SGC.Logica;

public class MedicoService
{
    public List<Medico> ObtenerTodos()
    {
        using var contexto = SGCContextFactory.Crear();
        var medicos = contexto.Medicos
            .Where(m => m.Activo)
            .Include(m => m.ObrasSocialesAceptadas)
            .ToList();

        medicos.ForEach(SincronizarIds);
        return medicos;
    }

    public Medico? ObtenerPorId(int id)
    {
        using var contexto = SGCContextFactory.Crear();
        var medico = contexto.Medicos
            .Include(m => m.ObrasSocialesAceptadas)
            .FirstOrDefault(m => m.Id == id);

        if (medico != null) SincronizarIds(medico);
        return medico;
    }

    // ObrasSocialesAceptadasIds es [NotMapped] (pegamento con la UI, el
    // CheckedListBox) - EF nunca lo llena solo al leer de la base, hay que
    // sincronizarlo a mano desde la navegacion real (ObrasSocialesAceptadas)
    // despues de cada consulta. Si no se hace esto, el checklist de
    // FormMedicos siempre aparece vacio al editar un medico existente,
    // aunque ya tenga obras sociales vinculadas en la base.
    private static void SincronizarIds(Medico medico)
    {
        medico.ObrasSocialesAceptadasIds = medico.ObrasSocialesAceptadas.Select(o => o.Id).ToList();
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
        if (string.IsNullOrWhiteSpace(medico.Nombre) || !medico.Nombre.All(c => char.IsLetter(c) || c == ' '))
            throw new ArgumentException("El nombre solo puede contener letras.");

        if (string.IsNullOrWhiteSpace(medico.Apellido) || !medico.Apellido.All(c => char.IsLetter(c) || c == ' '))
            throw new ArgumentException("El apellido solo puede contener letras.");

        if (string.IsNullOrWhiteSpace(medico.Dni) || !medico.Dni.All(char.IsDigit) ||
            medico.Dni.Length < 7 || medico.Dni.Length > 8)
            throw new ArgumentException("El DNI debe tener entre 7 y 8 digitos numericos, sin puntos ni letras.");

        // "0000000" pasa el chequeo de arriba (son 7 digitos numericos) pero
        // no es un DNI real - ningun documento existe con todos ceros.
        if (medico.Dni.All(c => c == '0'))
            throw new ArgumentException("El DNI ingresado no es valido.");

        if (string.IsNullOrWhiteSpace(medico.Matricula))
            throw new ArgumentException("La matricula es obligatoria.");

        if (string.IsNullOrWhiteSpace(medico.Especialidad) || !medico.Especialidad.All(c => char.IsLetter(c) || c == ' '))
            throw new ArgumentException("La especialidad solo puede contener letras.");

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
