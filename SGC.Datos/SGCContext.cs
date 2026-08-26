using Microsoft.EntityFrameworkCore;
using SGC.Entidades;

namespace SGC.Datos;

public class SGCContext : DbContext
{
    public SGCContext(DbContextOptions<SGCContext> options) : base(options)
    {
    }

    public DbSet<Paciente> Pacientes => Set<Paciente>();
    public DbSet<Medico> Medicos => Set<Medico>();
    public DbSet<Horario> Horarios => Set<Horario>();
    public DbSet<TipoActividad> TiposActividad => Set<TipoActividad>();
    public DbSet<AgendaMedico> AgendasMedico => Set<AgendaMedico>();
    public DbSet<Turno> Turnos => Set<Turno>();
    public DbSet<ActividadMedica> ActividadesMedicas => Set<ActividadMedica>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Evita el sobre-turno (RF#06 / riesgo "Doble asignación de turnos"):
        // no puede haber dos turnos activos para el mismo médico, mismo horario y misma fecha.
        modelBuilder.Entity<Turno>()
            .HasIndex(t => new { t.MedicoId, t.HorarioId, t.Fecha })
            .IsUnique()
            .HasFilter("[Activo] = 1");

        modelBuilder.Entity<Turno>()
            .HasOne(t => t.ActividadMedica)
            .WithOne(a => a.Turno)
            .HasForeignKey<ActividadMedica>(a => a.TurnoId);
    }
}
