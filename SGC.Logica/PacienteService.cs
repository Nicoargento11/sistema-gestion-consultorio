using Microsoft.EntityFrameworkCore;
using SGC.Datos;
using SGC.Entidades;

namespace SGC.Logica;

public class PacienteService
{
    public List<Paciente> ObtenerTodos()
    {
        using var contexto = SGCContextFactory.Crear();
        return contexto.Pacientes
            .Where(p => p.Activo)
            .Include(p => p.ObraSocial)
            .ToList();
    }

    public void Agregar(Paciente paciente)
    {
        using var contexto = SGCContextFactory.Crear();
        Validar(paciente, contexto);

        paciente.Id = 0;
        paciente.Activo = true;
        contexto.Pacientes.Add(paciente);
        contexto.SaveChanges();
    }

    public void Modificar(Paciente paciente)
    {
        using var contexto = SGCContextFactory.Crear();
        Validar(paciente, contexto);

        var existente = contexto.Pacientes.FirstOrDefault(p => p.Id == paciente.Id)
            ?? throw new InvalidOperationException("El paciente que intenta modificar no existe.");

        existente.Nombre = paciente.Nombre;
        existente.Apellido = paciente.Apellido;
        existente.Dni = paciente.Dni;
        existente.Email = paciente.Email;
        existente.Telefono = paciente.Telefono;
        existente.FechaNacimiento = paciente.FechaNacimiento;
        existente.ObraSocialId = paciente.ObraSocialId;

        contexto.SaveChanges();
    }

    public void EliminarLogico(int id)
    {
        using var contexto = SGCContextFactory.Crear();
        var paciente = contexto.Pacientes.FirstOrDefault(p => p.Id == id)
            ?? throw new InvalidOperationException("El paciente que intenta eliminar no existe.");

        paciente.Activo = false;
        contexto.SaveChanges();
    }

    private void Validar(Paciente paciente, SGCContext contexto)
    {
        if (string.IsNullOrWhiteSpace(paciente.Nombre) || !paciente.Nombre.All(c => char.IsLetter(c) || c == ' '))
            throw new ArgumentException("El nombre solo puede contener letras.");

        if (string.IsNullOrWhiteSpace(paciente.Apellido) || !paciente.Apellido.All(c => char.IsLetter(c) || c == ' '))
            throw new ArgumentException("El apellido solo puede contener letras.");

        if (string.IsNullOrWhiteSpace(paciente.Dni) || !paciente.Dni.All(char.IsDigit) ||
            paciente.Dni.Length < 7 || paciente.Dni.Length > 8)
            throw new ArgumentException("El DNI debe tener entre 7 y 8 dígitos numéricos, sin puntos ni letras.");

        // "0000000" pasa el chequeo de arriba (son 7 digitos numericos) pero
        // no es un DNI real - ningun documento existe con todos ceros.
        if (paciente.Dni.All(c => c == '0'))
            throw new ArgumentException("El DNI ingresado no es válido.");

        if (string.IsNullOrWhiteSpace(paciente.Email) || !paciente.Email.Contains('@') || !paciente.Email.Contains('.'))
            throw new ArgumentException("Debe ingresar un email con formato válido (ej: nombre@dominio.com).");

        if (string.IsNullOrWhiteSpace(paciente.Telefono) || !paciente.Telefono.All(char.IsDigit) ||
            paciente.Telefono.Length < 6 || paciente.Telefono.Length > 15)
            throw new ArgumentException("El teléfono debe tener entre 6 y 15 dígitos numéricos, sin espacios ni guiones.");

        if (paciente.FechaNacimiento == default)
            throw new ArgumentException("Debe ingresar la fecha de nacimiento.");

        if (paciente.FechaNacimiento > DateOnly.FromDateTime(DateTime.Today))
            throw new ArgumentException("La fecha de nacimiento no puede ser futura.");

        if (paciente.FechaNacimiento < DateOnly.FromDateTime(DateTime.Today).AddYears(-120))
            throw new ArgumentException("La fecha de nacimiento no es válida.");

        if (contexto.Pacientes.Any(p => p.Activo && p.Dni == paciente.Dni && p.Id != paciente.Id))
            throw new InvalidOperationException($"Ya existe un paciente activo con el DNI {paciente.Dni}.");

        // ObraSocialId es opcional: null se interpreta como "Particular".
        if (paciente.ObraSocialId is int obraSocialId)
        {
            var obraSocial = contexto.ObrasSociales.FirstOrDefault(o => o.Id == obraSocialId);
            if (obraSocial is null || !obraSocial.Activo)
                throw new ArgumentException("La obra social seleccionada no es valida.");
        }
    }
}
