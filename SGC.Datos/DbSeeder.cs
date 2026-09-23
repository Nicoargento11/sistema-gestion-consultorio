using SGC.Entidades;

namespace SGC.Datos;

// Datos de prueba para desarrollo y demostracion - antes vivian hardcodeados
// en cada Service (List<T> estatica), ahora se insertan una sola vez en la
// base real la primera vez que arranca la app contra una base vacia.
public static class DbSeeder
{
    public static void Sembrar(SGCContext contexto)
    {
        if (contexto.Medicos.Any())
            return;

        var osOsde = new ObraSocial { Nombre = "OSDE", PorcentajeCobertura = 70, Activo = true };
        var osSwiss = new ObraSocial { Nombre = "Swiss Medical", PorcentajeCobertura = 60, Activo = true };
        var osIoscor = new ObraSocial { Nombre = "IOSCOR", PorcentajeCobertura = 50, Activo = true };
        contexto.ObrasSociales.AddRange(osOsde, osSwiss, osIoscor);

        var medico1 = new Medico
        {
            Dni = "20111222", Nombre = "Laura", Apellido = "Gomez", Matricula = "MP1234",
            Especialidad = "Clinica General", PrecioConsultaParticular = 8000, Activo = true,
            ObrasSocialesAceptadas = new() { osOsde, osSwiss }
        };
        var medico2 = new Medico
        {
            Dni = "20333444", Nombre = "Juan", Apellido = "Perez", Matricula = "MP5678",
            Especialidad = "Cardiologia", PrecioConsultaParticular = 12000, Activo = true,
            ObrasSocialesAceptadas = new() { osOsde }
        };
        var medico3 = new Medico
        {
            Dni = "20555666", Nombre = "Maria", Apellido = "Fernandez", Matricula = "MP9012",
            Especialidad = "Pediatria", PrecioConsultaParticular = 9000, Activo = true,
            ObrasSocialesAceptadas = new() { osSwiss, osIoscor }
        };
        contexto.Medicos.AddRange(medico1, medico2, medico3);

        var paciente1 = new Paciente { Dni = "35123456", Nombre = "Carlos", Apellido = "Fernandez", Email = "carlos.f@email.com", Telefono = "3794123456", FechaNacimiento = new DateOnly(1990, 4, 12), ObraSocial = osOsde, Activo = true };
        var paciente2 = new Paciente { Dni = "38987654", Nombre = "Ana", Apellido = "Martinez", Email = "ana.martinez@email.com", Telefono = "3794987654", FechaNacimiento = new DateOnly(1985, 9, 3), ObraSocial = osSwiss, Activo = true };
        var paciente3 = new Paciente { Dni = "40555666", Nombre = "Luis", Apellido = "Torres", Email = "luis.torres@email.com", Telefono = "3794555666", FechaNacimiento = new DateOnly(1998, 1, 27), ObraSocial = osIoscor, Activo = true };
        var paciente4 = new Paciente { Dni = "42111222", Nombre = "Sofia", Apellido = "Herrera", Email = "sofia.herrera@email.com", Telefono = "3794111222", FechaNacimiento = new DateOnly(2001, 11, 15), ObraSocial = null, Activo = true };
        contexto.Pacientes.AddRange(paciente1, paciente2, paciente3, paciente4);

        // Las contrasenas de prueba se hashean igual que cualquier otra -
        // "admin123"/"recepcion123"/"medico123" siguen siendo las claves
        // para loguearse, solo que ahora no quedan escritas en texto plano.
        var admin = new Usuario { NombreUsuario = "admin", Contrasena = PasswordHasher.Hashear("admin123"), Rol = RolUsuario.Administrador, Activo = true };
        var recepcion = new Usuario { NombreUsuario = "recepcion", Contrasena = PasswordHasher.Hashear("recepcion123"), Rol = RolUsuario.Recepcionista, Activo = true, MedicosAsignados = new() { medico1, medico2 } };
        var usuarioMedico = new Usuario { NombreUsuario = "medico", Contrasena = PasswordHasher.Hashear("medico123"), Rol = RolUsuario.Medico, Medico = medico1, Activo = true };
        contexto.Usuarios.AddRange(admin, recepcion, usuarioMedico);

        // Agenda semanal basica para que los turnos de ejemplo caigan dentro
        // de un rango valido (MedicoAtiende) y para que se pueda probar
        // "asignar turno" desde el primer arranque sin cargar nada a mano.
        contexto.AgendasMedico.AddRange(
            new AgendaMedico { Medico = medico1, DiaSemana = DayOfWeek.Monday, HoraInicio = new TimeOnly(8, 0), HoraFin = new TimeOnly(13, 0), Activo = true },
            new AgendaMedico { Medico = medico1, DiaSemana = DayOfWeek.Tuesday, HoraInicio = new TimeOnly(8, 0), HoraFin = new TimeOnly(13, 0), Activo = true },
            new AgendaMedico { Medico = medico1, DiaSemana = DayOfWeek.Wednesday, HoraInicio = new TimeOnly(8, 0), HoraFin = new TimeOnly(13, 0), Activo = true },
            new AgendaMedico { Medico = medico1, DiaSemana = DayOfWeek.Thursday, HoraInicio = new TimeOnly(8, 0), HoraFin = new TimeOnly(13, 0), Activo = true },
            new AgendaMedico { Medico = medico1, DiaSemana = DayOfWeek.Friday, HoraInicio = new TimeOnly(8, 0), HoraFin = new TimeOnly(13, 0), Activo = true },
            new AgendaMedico { Medico = medico2, DiaSemana = DayOfWeek.Monday, HoraInicio = new TimeOnly(14, 0), HoraFin = new TimeOnly(18, 0), Activo = true },
            new AgendaMedico { Medico = medico2, DiaSemana = DayOfWeek.Wednesday, HoraInicio = new TimeOnly(14, 0), HoraFin = new TimeOnly(18, 0), Activo = true },
            new AgendaMedico { Medico = medico3, DiaSemana = DayOfWeek.Tuesday, HoraInicio = new TimeOnly(9, 0), HoraFin = new TimeOnly(12, 0), Activo = true },
            new AgendaMedico { Medico = medico3, DiaSemana = DayOfWeek.Thursday, HoraInicio = new TimeOnly(9, 0), HoraFin = new TimeOnly(12, 0), Activo = true }
        );

        var hoy = DateOnly.FromDateTime(DateTime.Today);
        var turno1 = new Turno { Paciente = paciente1, Medico = medico1, Fecha = hoy, HoraInicio = new TimeOnly(9, 0), DuracionMinutos = 30, Estado = EstadoTurno.Confirmado, Activo = true };
        var turno2 = new Turno { Paciente = paciente2, Medico = medico1, Fecha = hoy, HoraInicio = new TimeOnly(9, 30), DuracionMinutos = 30, Estado = EstadoTurno.Confirmado, Activo = true };
        var turno3 = new Turno { Paciente = paciente3, Medico = medico1, Fecha = hoy, HoraInicio = new TimeOnly(10, 0), DuracionMinutos = 30, Estado = EstadoTurno.Confirmado, Activo = true };
        var turno4 = new Turno { Paciente = paciente4, Medico = medico1, Fecha = hoy, HoraInicio = new TimeOnly(10, 30), DuracionMinutos = 30, Estado = EstadoTurno.Confirmado, Activo = true };
        contexto.Turnos.AddRange(turno1, turno2, turno3, turno4);

        contexto.SaveChanges();

        var tipoConsultaGeneral = contexto.TiposActividad.First(t => t.NombreTipo == "Consulta General");
        contexto.ActividadesMedicas.Add(new ActividadMedica
        {
            Turno = turno1,
            TipoActividad = tipoConsultaGeneral,
            MotivoConsulta = "Control clinico general y chequeo anual de rutina.",
            Procedimiento = "Examen fisico completo: presion arterial 120/80 mmHg, auscultacion cardiaca y respiratoria normal. Sin hallazgos patologicos.",
            RecetaMedicamentos = "Solicitud de laboratorio de sangre y orina completo de rutina.",
            Activo = true
        });

        contexto.SaveChanges();
    }
}
