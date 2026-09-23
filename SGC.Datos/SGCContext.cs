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
    public DbSet<TipoActividad> TiposActividad => Set<TipoActividad>();
    public DbSet<AgendaMedico> AgendasMedico => Set<AgendaMedico>();
    public DbSet<ExcepcionAgenda> ExcepcionesAgenda => Set<ExcepcionAgenda>();
    public DbSet<Turno> Turnos => Set<Turno>();
    public DbSet<ActividadMedica> ActividadesMedicas => Set<ActividadMedica>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    // Horario NO se persiste: HorarioService la usa solo como generador de
    // slots en memoria a partir de AgendaMedico (ver nota en HorarioService),
    // nunca sobrevive a un reinicio ni la referencia Turno.

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Medico>(e =>
        {
            e.Property(m => m.Dni).IsRequired().HasMaxLength(15);
            e.HasIndex(m => m.Dni).IsUnique();
            e.Property(m => m.Nombre).IsRequired().HasMaxLength(80);
            e.Property(m => m.Apellido).IsRequired().HasMaxLength(80);
            e.Property(m => m.Matricula).IsRequired().HasMaxLength(30);
            e.HasIndex(m => m.Matricula).IsUnique();
            e.Property(m => m.Especialidad).IsRequired().HasMaxLength(80);
            e.Property(m => m.PrecioConsultaParticular).HasColumnType("decimal(10,2)");
            e.ToTable(t => t.HasCheckConstraint("CK_Medico_Precio", "PrecioConsultaParticular >= 0"));

            // M:N real: EF genera y administra la tabla intermedia MedicoObraSocial.
            e.HasMany(m => m.ObrasSocialesAceptadas)
                .WithMany()
                .UsingEntity(j => j.ToTable("MedicoObraSocial"));
        });

        modelBuilder.Entity<ObraSocial>(e =>
        {
            e.Property(o => o.Nombre).IsRequired().HasMaxLength(100);
            e.HasIndex(o => o.Nombre).IsUnique();
            e.Property(o => o.PorcentajeCobertura).HasColumnType("decimal(5,2)");
            e.ToTable(t => t.HasCheckConstraint("CK_ObraSocial_Porcentaje", "PorcentajeCobertura BETWEEN 0 AND 100"));
        });

        modelBuilder.Entity<Paciente>(e =>
        {
            e.Property(p => p.Dni).IsRequired().HasMaxLength(15);
            e.HasIndex(p => p.Dni).IsUnique();
            e.Property(p => p.Nombre).IsRequired().HasMaxLength(80);
            e.Property(p => p.Apellido).IsRequired().HasMaxLength(80);
            e.Property(p => p.Email).IsRequired().HasMaxLength(120);
            e.Property(p => p.Telefono).IsRequired().HasMaxLength(30);

            // Restrict (no Cascade): las bajas son logicas (Activo=false) en
            // toda la app, nunca se borra fisico - y Medico/Paciente son
            // referenciados desde varias tablas, asi que Cascade chocaria
            // con la regla de SQL Server que prohibe multiples caminos de
            // cascada hacia la misma tabla.
            e.HasOne(p => p.ObraSocial).WithMany().HasForeignKey(p => p.ObraSocialId).OnDelete(DeleteBehavior.Restrict);

            e.ToTable(t => t.HasCheckConstraint("CK_Paciente_FechaNacimiento", "FechaNacimiento <= CAST(GETDATE() AS DATE)"));
        });

        modelBuilder.Entity<Usuario>(e =>
        {
            e.Property(u => u.NombreUsuario).IsRequired().HasMaxLength(50);
            e.HasIndex(u => u.NombreUsuario).IsUnique();
            e.Property(u => u.Contrasena).IsRequired().HasMaxLength(200);

            e.HasOne(u => u.Medico).WithMany().HasForeignKey(u => u.MedicoId).OnDelete(DeleteBehavior.Restrict);

            // Un medico tiene a lo sumo un usuario de Rol=Medico (indice
            // filtrado, un UNIQUE comun no alcanza porque MedicoId es NULL
            // para Administrador/Recepcionista). 2 = RolUsuario.Medico.
            e.HasIndex(u => u.MedicoId)
                .IsUnique()
                .HasFilter("[Rol] = 2")
                .HasDatabaseName("UQ_Usuario_MedicoUnico");

            e.ToTable(t => t.HasCheckConstraint("CK_Usuario_RolMedico", "Rol <> 2 OR MedicoId IS NOT NULL"));

            // M:N real: EF genera y administra la tabla intermedia UsuarioMedico.
            e.HasMany(u => u.MedicosAsignados)
                .WithMany()
                .UsingEntity(j => j.ToTable("UsuarioMedico"));
        });

        modelBuilder.Entity<AgendaMedico>(e =>
        {
            e.HasOne(a => a.Medico).WithMany(m => m.Agenda).HasForeignKey(a => a.MedicoId).OnDelete(DeleteBehavior.Restrict);
            e.ToTable(t => t.HasCheckConstraint("CK_AgendaMedico_Rango", "HoraFin > HoraInicio"));
            e.HasIndex(a => new { a.MedicoId, a.DiaSemana, a.HoraInicio, a.HoraFin }).IsUnique();
        });

        modelBuilder.Entity<ExcepcionAgenda>(e =>
        {
            e.HasOne(x => x.Medico).WithMany().HasForeignKey(x => x.MedicoId).OnDelete(DeleteBehavior.Restrict);
            e.Property(x => x.Motivo).HasMaxLength(200);

            // Tipo 0=DiaCompleto (sin horas) / 1=RangoHorario (con horas y HoraFin > HoraInicio).
            e.ToTable(t => t.HasCheckConstraint("CK_ExcepcionAgenda_Tipo",
                "(Tipo = 0 AND HoraInicio IS NULL AND HoraFin IS NULL) OR " +
                "(Tipo = 1 AND HoraInicio IS NOT NULL AND HoraFin IS NOT NULL AND HoraFin > HoraInicio)"));
        });

        modelBuilder.Entity<Turno>(e =>
        {
            e.HasOne(t => t.Paciente).WithMany(p => p.Turnos).HasForeignKey(t => t.PacienteId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(t => t.Medico).WithMany(m => m.Turnos).HasForeignKey(t => t.MedicoId).OnDelete(DeleteBehavior.Restrict);

            e.Property(t => t.DuracionMinutos).HasDefaultValue(30);
            e.Property(t => t.Monto).HasColumnType("decimal(10,2)");
            e.Property(t => t.MedioPago).HasMaxLength(30);

            e.ToTable(t => t.HasCheckConstraint("CK_Turno_Duracion", "DuracionMinutos > 0"));
            e.ToTable(t => t.HasCheckConstraint("CK_Turno_Monto", "Monto IS NULL OR Monto >= 0"));

            e.HasOne(t => t.ActividadMedica)
                .WithOne(a => a.Turno)
                .HasForeignKey<ActividadMedica>(a => a.TurnoId);
        });

        modelBuilder.Entity<ActividadMedica>(e =>
        {
            e.HasOne(a => a.TipoActividad).WithMany().HasForeignKey(a => a.TipoActividadId).OnDelete(DeleteBehavior.Restrict);
            e.Property(a => a.MotivoConsulta).IsRequired().HasMaxLength(500);
            e.Property(a => a.RecetaMedicamentos).HasMaxLength(500);
            e.Property(a => a.Procedimiento).HasMaxLength(500);
            e.ToTable(t => t.HasCheckConstraint("CK_ActividadMedica_Motivo", "LEN(MotivoConsulta) > 0"));
        });

        modelBuilder.Entity<TipoActividad>(e =>
        {
            e.Property(t => t.NombreTipo).IsRequired().HasMaxLength(80);
            e.HasIndex(t => t.NombreTipo).IsUnique();
            e.Property(t => t.DuracionSugeridaMinutos).HasDefaultValue(30);
            e.ToTable(t => t.HasCheckConstraint("CK_TipoActividad_Duracion", "DuracionSugeridaMinutos > 0"));

            // Catalogo fijo de tipos de actividad - se siembra por migracion
            // (HasData) en vez de por codigo en runtime, para que quede
            // versionado igual que el resto del esquema.
            e.HasData(
                new TipoActividad { Id = 1, NombreTipo = "Consulta General", Descripcion = "Atencion clinica de rutina o primera vez", DuracionSugeridaMinutos = 30, Activo = true },
                new TipoActividad { Id = 2, NombreTipo = "Control / Seguimiento", Descripcion = "Control periodico o post-tratamiento", DuracionSugeridaMinutos = 30, Activo = true },
                new TipoActividad { Id = 3, NombreTipo = "Estudio / Practica", Descripcion = "Realizacion o evaluacion de estudios clinicos", DuracionSugeridaMinutos = 45, Activo = true },
                new TipoActividad { Id = 4, NombreTipo = "Receta / Prescripcion", Descripcion = "Emision o renovacion de recetas farmacologicas", DuracionSugeridaMinutos = 20, Activo = true },
                new TipoActividad { Id = 5, NombreTipo = "Certificado Medico", Descripcion = "Emision de apto fisico o certificado medico", DuracionSugeridaMinutos = 20, Activo = true }
            );
        });
    }
}
