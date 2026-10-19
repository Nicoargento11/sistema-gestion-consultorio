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

    // Listado para la grilla de Pacientes con ultima visita, proximo turno y
    // ausencias. Son 3 consultas en total (pacientes, agregados por paciente y
    // turnos futuros), nunca una por fila.
    // medicosVisibles limita SOLO el "proximo turno" a esos medicos (para una
    // recepcionista: los que tiene asignados; lista vacia = ninguno). null
    // significa sin restriccion (administrador).
    public List<PacienteFila> ObtenerListado(IReadOnlyCollection<int>? medicosVisibles = null)
    {
        using var contexto = SGCContextFactory.Crear();

        var pacientes = contexto.Pacientes
            .Where(p => p.Activo)
            .Include(p => p.ObraSocial)
            .ToList();

        // Ultima visita y ausencias: agrupado en SQL, una fila por paciente.
        var agregados = contexto.Turnos
            .Where(t => t.Activo && (t.Estado == EstadoTurno.Asistio || t.Estado == EstadoTurno.Ausente))
            .GroupBy(t => t.PacienteId)
            .Select(g => new
            {
                PacienteId = g.Key,
                UltimaVisita = g.Max(t => t.Estado == EstadoTurno.Asistio ? (DateOnly?)t.Fecha : null),
                Ausencias = g.Count(t => t.Estado == EstadoTurno.Ausente)
            })
            .ToDictionary(a => a.PacienteId);

        // Proximo turno: se traen solo columnas livianas de los turnos futuros
        // y se queda con el primero de cada paciente. Un consultorio tiene
        // pocos turnos futuros abiertos, asi que es mas simple que una
        // subconsulta correlacionada y sigue siendo una unica consulta.
        var hoy = DateOnly.FromDateTime(DateTime.Today);
        var ahora = TimeOnly.FromDateTime(DateTime.Now);
        var proximos = new Dictionary<int, string>();
        if (medicosVisibles == null || medicosVisibles.Count > 0)
        {
            var futuros = contexto.Turnos
                .Where(t => t.Activo
                            && (t.Estado == EstadoTurno.Pendiente || t.Estado == EstadoTurno.Confirmado)
                            && (t.Fecha > hoy || (t.Fecha == hoy && t.HoraInicio >= ahora))
                            && (medicosVisibles == null || medicosVisibles.Contains(t.MedicoId)))
                .OrderBy(t => t.Fecha).ThenBy(t => t.HoraInicio)
                .Select(t => new { t.PacienteId, t.Fecha, t.HoraInicio, MedicoApellido = t.Medico!.Apellido, MedicoNombre = t.Medico!.Nombre })
                .ToList();

            foreach (var f in futuros)
            {
                if (!proximos.ContainsKey(f.PacienteId))
                    proximos[f.PacienteId] = $"{f.Fecha:dd/MM/yyyy} {f.HoraInicio:HH:mm} - {f.MedicoApellido}, {f.MedicoNombre}";
            }
        }

        return pacientes.Select(p =>
        {
            agregados.TryGetValue(p.Id, out var agg);
            return new PacienteFila
            {
                Paciente = p,
                UltimaVisita = agg?.UltimaVisita,
                Ausencias = agg?.Ausencias ?? 0,
                ProximoTurno = proximos.TryGetValue(p.Id, out var texto) ? texto : "-"
            };
        }).ToList();
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
