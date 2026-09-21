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
    public DbSet<ObraSocial> ObrasSociales => Set<ObraSocial>();
    public DbSet<Horario> Horarios => Set<Horario>();
    public DbSet<TipoActividad> TiposActividad => Set<TipoActividad>();
    public DbSet<AgendaMedico> AgendasMedico => Set<AgendaMedico>();
    public DbSet<ExcepcionAgenda> ExcepcionesAgenda => Set<ExcepcionAgenda>();
    public DbSet<Turno> Turnos => Set<Turno>();
    public DbSet<ActividadMedica> ActividadesMedicas => Set<ActividadMedica>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Turno>()
            .HasOne(t => t.ActividadMedica)
            .WithOne(a => a.Turno)
            .HasForeignKey<ActividadMedica>(a => a.TurnoId);

        modelBuilder.Entity<Turno>()
            .Property(t => t.DuracionMinutos)
            .HasDefaultValue(30);

        modelBuilder.Entity<TipoActividad>()
            .Property(t => t.DuracionSugeridaMinutos)
            .HasDefaultValue(30);
    }
}
