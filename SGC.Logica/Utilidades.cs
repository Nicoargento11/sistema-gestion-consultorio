using System.Net.Mail;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SGC.Datos;
using SGC.Entidades;

namespace SGC.Logica;

// Piezas compartidas por los services (validacion de textos, aritmetica de
// horas y manejo de errores de la base). Son internas: la UI no las usa.
internal static partial class Texto
{
    [GeneratedRegex(@"\s+")]
    private static partial Regex EspaciosMultiples();

    [GeneratedRegex(@"^\+?[\d\s\-()]+$")]
    private static partial Regex FormatoTelefono();

    // Trim + colapsa espacios repetidos en uno solo. null se trata como "".
    public static string Normalizar(string? valor)
        => EspaciosMultiples().Replace((valor ?? string.Empty).Trim(), " ");

    // Letras (con acentos y enie), espacios, apostrofe y guion; al menos una letra.
    public static bool EsNombreValido(string valor)
        => valor.Any(char.IsLetter)
           && valor.All(c => char.IsLetter(c) || c == ' ' || c == '\'' || c == '’' || c == '-');

    public static bool EsEmailValido(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor) || valor.Length > 120 || valor.Any(char.IsWhiteSpace))
            return false;

        try
        {
            var direccion = new MailAddress(valor);
            return direccion.Address == valor
                   && direccion.Host.Contains('.')
                   && !direccion.Host.StartsWith('.')
                   && !direccion.Host.EndsWith('.');
        }
        catch (FormatException)
        {
            return false;
        }
    }

    // Acepta digitos mas + espacio - ( ) opcionales, con 6 a 15 digitos en
    // total. Devuelve solo los digitos (la base exige solo digitos), o null
    // si no es valido.
    public static string? TelefonoADigitos(string valor)
    {
        if (!FormatoTelefono().IsMatch(valor)) return null;

        var digitos = new string(valor.Where(char.IsDigit).ToArray());
        return digitos.Length is >= 6 and <= 15 ? digitos : null;
    }
}

internal static class Horas
{
    // Las comparaciones de rangos se hacen en minutos desde las 00:00: sumar
    // minutos a un TimeOnly da la vuelta pasada la medianoche (23:30 + 45 min
    // = 00:15) y rompe los operadores < y >.
    public static int Minutos(TimeOnly hora) => hora.Hour * 60 + hora.Minute;

    public static bool Superpone(int inicioA, int finA, int inicioB, int finB)
        => inicioA < finB && inicioB < finA;

    public static TimeOnly ADesdeMinutos(int minutos)
        => TimeOnly.FromTimeSpan(TimeSpan.FromMinutes(minutos));
}

internal static class Persistencia
{
    // SaveChanges que convierte una violacion de indice unico (2601/2627) en
    // un mensaje claro en espanol; nunca se expone el mensaje de EF/SQL.
    public static void Guardar(SGCContext contexto)
    {
        try
        {
            contexto.SaveChanges();
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sql && (sql.Number == 2601 || sql.Number == 2627))
        {
            throw new InvalidOperationException(MensajeDuplicado(sql.Message), ex);
        }
    }

    private static string MensajeDuplicado(string detalle)
    {
        if (detalle.Contains("IX_Medicos_Dni")) return "Ya existe un médico con ese DNI (puede estar dado de baja).";
        if (detalle.Contains("IX_Medicos_Matricula")) return "Ya existe un médico con esa matrícula (puede estar dado de baja).";
        if (detalle.Contains("IX_Pacientes_Dni")) return "Ya existe un paciente con ese DNI (puede estar dado de baja).";
        if (detalle.Contains("IX_ObrasSociales_Nombre")) return "Ya existe una obra social con ese nombre (puede estar dada de baja).";
        if (detalle.Contains("UQ_Usuario_MedicoUnico")) return "Ese médico ya tiene un usuario (puede estar dado de baja).";
        if (detalle.Contains("IX_Usuarios_NombreUsuario")) return "Ya existe un usuario con ese nombre (puede estar dado de baja).";
        if (detalle.Contains("IX_AgendasMedico")) return "Ya existe una franja de agenda idéntica para ese médico y día (puede estar dada de baja).";
        return "Ya existe un registro con esos datos (puede estar dado de baja).";
    }
}

internal static class TurnosFuturos
{
    // Turnos confirmados que todavia no ocurrieron (de hoy en adelante, hoy
    // solo si la hora de inicio no paso).
    public static IQueryable<Turno> Confirmados(SGCContext contexto)
    {
        var hoy = DateOnly.FromDateTime(DateTime.Today);
        var ahora = TimeOnly.FromDateTime(DateTime.Now);
        return contexto.Turnos.Where(t => t.Activo
            && t.Estado == EstadoTurno.Confirmado
            && (t.Fecha > hoy || (t.Fecha == hoy && t.HoraInicio >= ahora)));
    }
}
