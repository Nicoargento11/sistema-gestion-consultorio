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

    // Listado con obras sociales aceptadas y turnos de hoy / del mes. Dos
    // consultas (medicos con sus obras, conteo de turnos agrupado), sin
    // consultas por fila.
    public List<MedicoFila> ObtenerListado()
    {
        using var contexto = SGCContextFactory.Crear();

        var medicos = contexto.Medicos
            .AsNoTracking()
            .Where(m => m.Activo)
            .Include(m => m.ObrasSocialesAceptadas)
            .ToList();
        medicos.ForEach(SincronizarIds);

        var hoy = DateOnly.FromDateTime(DateTime.Today);
        var inicioMes = new DateOnly(hoy.Year, hoy.Month, 1);
        var finMes = inicioMes.AddMonths(1).AddDays(-1);

        var conteos = contexto.Turnos
            .AsNoTracking()
            .Where(t => t.Activo && t.Estado != EstadoTurno.Cancelado
                        && t.Fecha >= inicioMes && t.Fecha <= finMes)
            .GroupBy(t => t.MedicoId)
            .Select(g => new { MedicoId = g.Key, Mes = g.Count(), Hoy = g.Count(t => t.Fecha == hoy) })
            .ToDictionary(x => x.MedicoId);

        return medicos.Select(m => new MedicoFila
        {
            Medico = m,
            ObrasSociales = m.ObrasSocialesAceptadas
                .Where(o => o.Activo)
                .Select(o => o.Nombre)
                .OrderBy(n => n, StringComparer.CurrentCultureIgnoreCase)
                .ToList(),
            TurnosDelMes = conteos.TryGetValue(m.Id, out var c) ? c.Mes : 0,
            TurnosDeHoy = conteos.TryGetValue(m.Id, out var c2) ? c2.Hoy : 0
        }).ToList();
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
        Persistencia.Guardar(contexto);
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

        Persistencia.Guardar(contexto);
    }

    public void EliminarLogico(int id)
    {
        using var contexto = SGCContextFactory.Crear();
        var medico = contexto.Medicos.FirstOrDefault(m => m.Id == id)
            ?? throw new InvalidOperationException("El medico que intenta eliminar no existe.");

        int turnosFuturos = TurnosFuturos.Confirmados(contexto).Count(t => t.MedicoId == id);
        if (turnosFuturos > 0)
            throw new InvalidOperationException(
                $"No se puede dar de baja al médico: tiene {turnosFuturos} turno(s) confirmado(s) pendiente(s). Cancele o reasigne esos turnos primero.");

        medico.Activo = false;

        // Su usuario de acceso (Rol Medico) queda inactivo en la misma operacion.
        foreach (var usuario in contexto.Usuarios.Where(u => u.Activo && u.Rol == RolUsuario.Medico && u.MedicoId == id))
            usuario.Activo = false;

        contexto.SaveChanges();
    }

    private void Validar(Medico medico, SGCContext contexto)
    {
        medico.Nombre = Texto.Normalizar(medico.Nombre);
        medico.Apellido = Texto.Normalizar(medico.Apellido);
        medico.Especialidad = Texto.Normalizar(medico.Especialidad);
        medico.Matricula = Texto.Normalizar(medico.Matricula);
        medico.Dni = (medico.Dni ?? string.Empty).Trim();

        if (!Texto.EsNombreValido(medico.Nombre) || medico.Nombre.Length > 80)
            throw new ArgumentException("El nombre solo puede contener letras, espacios, apóstrofes y guiones (máximo 80 caracteres).");

        if (!Texto.EsNombreValido(medico.Apellido) || medico.Apellido.Length > 80)
            throw new ArgumentException("El apellido solo puede contener letras, espacios, apóstrofes y guiones (máximo 80 caracteres).");

        if (string.IsNullOrWhiteSpace(medico.Dni) || !medico.Dni.All(char.IsDigit) ||
            medico.Dni.Length < 7 || medico.Dni.Length > 8)
            throw new ArgumentException("El DNI debe tener entre 7 y 8 dígitos numéricos, sin puntos ni letras.");

        // "0000000" pasa el chequeo de arriba (son 7 digitos numericos) pero
        // no es un DNI real - ningun documento existe con todos ceros.
        if (medico.Dni.All(c => c == '0'))
            throw new ArgumentException("El DNI ingresado no es válido.");

        if (medico.Matricula.Length == 0)
            throw new ArgumentException("La matrícula es obligatoria.");

        if (medico.Matricula.Length > 30)
            throw new ArgumentException("La matrícula no puede superar los 30 caracteres.");

        if (medico.Especialidad.Length == 0 || medico.Especialidad.Length > 80 ||
            !medico.Especialidad.All(c => char.IsLetter(c) || c == ' '))
            throw new ArgumentException("La especialidad solo puede contener letras (máximo 80 caracteres).");

        if (medico.PrecioConsultaParticular < 0)
            throw new ArgumentException("El precio de consulta particular no puede ser negativo.");

        // Los indices unicos de la base no distinguen activos de dados de baja,
        // asi que el chequeo tampoco lo hace (pero el mensaje si).
        var mismoDni = contexto.Medicos.FirstOrDefault(m => m.Dni == medico.Dni && m.Id != medico.Id);
        if (mismoDni != null)
            throw new InvalidOperationException(mismoDni.Activo
                ? $"Ya existe un médico con el DNI {medico.Dni}."
                : $"Ya existe un médico con el DNI {medico.Dni} (está dado de baja).");

        var mismaMatricula = contexto.Medicos.FirstOrDefault(m => m.Matricula == medico.Matricula && m.Id != medico.Id);
        if (mismaMatricula != null)
            throw new InvalidOperationException(mismaMatricula.Activo
                ? $"Ya existe un médico con la matrícula {medico.Matricula}."
                : $"Ya existe un médico con la matrícula {medico.Matricula} (está dado de baja).");

        foreach (var obraSocialId in medico.ObrasSocialesAceptadasIds)
        {
            var obraSocial = contexto.ObrasSociales.FirstOrDefault(o => o.Id == obraSocialId);
            if (obraSocial is null || !obraSocial.Activo)
                throw new ArgumentException("Seleccionó una obra social que no es válida.");
        }
    }
}
