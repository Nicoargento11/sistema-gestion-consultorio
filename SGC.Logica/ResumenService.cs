using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SGC.Datos;
using SGC.Entidades;
using SGC.Entidades.Reportes;

namespace SGC.Logica;

// Datos de la pantalla de inicio. Los contadores vienen de sp_ResumenDelDia
// (migracion AgregaSpResumenDelDia); las listas cortas (proximo paciente,
// turnos sin confirmar) se consultan con EF. Ningun metodo devuelve datos
// clinicos (RNF#07). Limitar que medicos puede ver cada usuario es
// responsabilidad de quien llama (medicoId / medicosIds).
public class ResumenService
{
    // medicoId null = todos los medicos.
    public ResumenDelDiaFila ObtenerResumen(DateOnly fecha, int? medicoId = null)
    {
        using var contexto = SGCContextFactory.Crear();
        return contexto.Database.SqlQueryRaw<ResumenDelDiaFila>(
                "EXEC sp_ResumenDelDia @Fecha, @MedicoId",
                new SqlParameter("@Fecha", System.Data.SqlDbType.Date) { Value = fecha.ToDateTime(TimeOnly.MinValue) },
                new SqlParameter("@MedicoId", System.Data.SqlDbType.Int) { Value = medicoId.HasValue ? medicoId.Value : DBNull.Value })
            .AsEnumerable()
            .FirstOrDefault() ?? new ResumenDelDiaFila();
    }

    // Une el resumen de varios medicos (recepcionista: solo los asignados).
    // Una lista vacia da un resumen en cero, nunca "todos los medicos".
    public ResumenDelDiaFila ObtenerResumenDeMedicos(DateOnly fecha, IEnumerable<int> medicosIds)
    {
        var total = new ResumenDelDiaFila();
        foreach (int id in medicosIds.Distinct())
            total.Sumar(ObtenerResumen(fecha, id));
        return total;
    }

    // Primer paciente que todavia falta atender ese dia (Pendiente o
    // Confirmado) a partir de la hora indicada; null si no queda ninguno.
    public Turno? ObtenerProximoPaciente(int medicoId, DateOnly fecha, TimeOnly desdeHora)
    {
        using var contexto = SGCContextFactory.Crear();
        return contexto.Turnos
            .Include(t => t.Paciente)
            .Where(t => t.MedicoId == medicoId
                        && t.Activo
                        && t.Fecha == fecha
                        && t.HoraInicio >= desdeHora
                        && (t.Estado == EstadoTurno.Pendiente || t.Estado == EstadoTurno.Confirmado))
            .OrderBy(t => t.HoraInicio)
            .FirstOrDefault();
    }

    // Proximos turnos Pendientes (sin confirmar) desde la fecha indicada.
    public List<Turno> ObtenerProximosSinConfirmar(DateOnly desde, IEnumerable<int> medicosIds, int cantidad = 8)
    {
        var ids = medicosIds.Distinct().ToList();
        if (ids.Count == 0 || cantidad <= 0)
            return new List<Turno>();

        using var contexto = SGCContextFactory.Crear();
        return contexto.Turnos
            .Include(t => t.Paciente)
            .Include(t => t.Medico)
            .Where(t => t.Activo
                        && t.Estado == EstadoTurno.Pendiente
                        && t.Fecha >= desde
                        && ids.Contains(t.MedicoId))
            .OrderBy(t => t.Fecha).ThenBy(t => t.HoraInicio)
            .Take(cantidad)
            .ToList();
    }
}
