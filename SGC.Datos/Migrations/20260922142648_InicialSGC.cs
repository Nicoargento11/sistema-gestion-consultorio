using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SGC.Datos.Migrations
{
    /// <inheritdoc />
    public partial class InicialSGC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Medicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Dni = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Matricula = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Especialidad = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    PrecioConsultaParticular = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicos", x => x.Id);
                    table.CheckConstraint("CK_Medico_Precio", "PrecioConsultaParticular >= 0");
                });

            migrationBuilder.CreateTable(
                name: "ObrasSociales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PorcentajeCobertura = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObrasSociales", x => x.Id);
                    table.CheckConstraint("CK_ObraSocial_Porcentaje", "PorcentajeCobertura BETWEEN 0 AND 100");
                });

            migrationBuilder.CreateTable(
                name: "TiposActividad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreTipo = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DuracionSugeridaMinutos = table.Column<int>(type: "int", nullable: false, defaultValue: 30),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposActividad", x => x.Id);
                    table.CheckConstraint("CK_TipoActividad_Duracion", "DuracionSugeridaMinutos > 0");
                });

            migrationBuilder.CreateTable(
                name: "AgendasMedico",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicoId = table.Column<int>(type: "int", nullable: false),
                    HoraInicio = table.Column<TimeOnly>(type: "time", nullable: false),
                    HoraFin = table.Column<TimeOnly>(type: "time", nullable: false),
                    DiaSemana = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgendasMedico", x => x.Id);
                    table.CheckConstraint("CK_AgendaMedico_Rango", "HoraFin > HoraInicio");
                    table.ForeignKey(
                        name: "FK_AgendasMedico_Medicos_MedicoId",
                        column: x => x.MedicoId,
                        principalTable: "Medicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExcepcionesAgenda",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicoId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    HoraInicio = table.Column<TimeOnly>(type: "time", nullable: true),
                    HoraFin = table.Column<TimeOnly>(type: "time", nullable: true),
                    Motivo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcepcionesAgenda", x => x.Id);
                    table.CheckConstraint("CK_ExcepcionAgenda_Tipo", "(Tipo = 0 AND HoraInicio IS NULL AND HoraFin IS NULL) OR (Tipo = 1 AND HoraInicio IS NOT NULL AND HoraFin IS NOT NULL AND HoraFin > HoraInicio)");
                    table.ForeignKey(
                        name: "FK_ExcepcionesAgenda_Medicos_MedicoId",
                        column: x => x.MedicoId,
                        principalTable: "Medicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreUsuario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Contrasena = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Rol = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    MedicoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.CheckConstraint("CK_Usuario_RolMedico", "Rol <> 2 OR MedicoId IS NOT NULL");
                    table.ForeignKey(
                        name: "FK_Usuarios_Medicos_MedicoId",
                        column: x => x.MedicoId,
                        principalTable: "Medicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MedicoObraSocial",
                columns: table => new
                {
                    MedicoId = table.Column<int>(type: "int", nullable: false),
                    ObrasSocialesAceptadasId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicoObraSocial", x => new { x.MedicoId, x.ObrasSocialesAceptadasId });
                    table.ForeignKey(
                        name: "FK_MedicoObraSocial_Medicos_MedicoId",
                        column: x => x.MedicoId,
                        principalTable: "Medicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicoObraSocial_ObrasSociales_ObrasSocialesAceptadasId",
                        column: x => x.ObrasSocialesAceptadasId,
                        principalTable: "ObrasSociales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pacientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Dni = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    FechaNacimiento = table.Column<DateOnly>(type: "date", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    ObraSocialId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacientes", x => x.Id);
                    table.CheckConstraint("CK_Paciente_FechaNacimiento", "FechaNacimiento <= CAST(GETDATE() AS DATE)");
                    table.ForeignKey(
                        name: "FK_Pacientes_ObrasSociales_ObraSocialId",
                        column: x => x.ObraSocialId,
                        principalTable: "ObrasSociales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioMedico",
                columns: table => new
                {
                    MedicosAsignadosId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioMedico", x => new { x.MedicosAsignadosId, x.UsuarioId });
                    table.ForeignKey(
                        name: "FK_UsuarioMedico_Medicos_MedicosAsignadosId",
                        column: x => x.MedicosAsignadosId,
                        principalTable: "Medicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioMedico_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Turnos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PacienteId = table.Column<int>(type: "int", nullable: false),
                    MedicoId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    HoraInicio = table.Column<TimeOnly>(type: "time", nullable: false),
                    DuracionMinutos = table.Column<int>(type: "int", nullable: false, defaultValue: 30),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    MedioPago = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Monto = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turnos", x => x.Id);
                    table.CheckConstraint("CK_Turno_Duracion", "DuracionMinutos > 0");
                    table.CheckConstraint("CK_Turno_Monto", "Monto IS NULL OR Monto >= 0");
                    table.ForeignKey(
                        name: "FK_Turnos_Medicos_MedicoId",
                        column: x => x.MedicoId,
                        principalTable: "Medicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Turnos_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActividadesMedicas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TurnoId = table.Column<int>(type: "int", nullable: false),
                    TipoActividadId = table.Column<int>(type: "int", nullable: false),
                    MotivoConsulta = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    RecetaMedicamentos = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Procedimiento = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActividadesMedicas", x => x.Id);
                    table.CheckConstraint("CK_ActividadMedica_Motivo", "LEN(MotivoConsulta) > 0");
                    table.ForeignKey(
                        name: "FK_ActividadesMedicas_TiposActividad_TipoActividadId",
                        column: x => x.TipoActividadId,
                        principalTable: "TiposActividad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActividadesMedicas_Turnos_TurnoId",
                        column: x => x.TurnoId,
                        principalTable: "Turnos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TiposActividad",
                columns: new[] { "Id", "Activo", "Descripcion", "DuracionSugeridaMinutos", "NombreTipo" },
                values: new object[,]
                {
                    { 1, true, "Atencion clinica de rutina o primera vez", 30, "Consulta General" },
                    { 2, true, "Control periodico o post-tratamiento", 30, "Control / Seguimiento" },
                    { 3, true, "Realizacion o evaluacion de estudios clinicos", 45, "Estudio / Practica" },
                    { 4, true, "Emision o renovacion de recetas farmacologicas", 20, "Receta / Prescripcion" },
                    { 5, true, "Emision de apto fisico o certificado medico", 20, "Certificado Medico" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActividadesMedicas_TipoActividadId",
                table: "ActividadesMedicas",
                column: "TipoActividadId");

            migrationBuilder.CreateIndex(
                name: "IX_ActividadesMedicas_TurnoId",
                table: "ActividadesMedicas",
                column: "TurnoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AgendasMedico_MedicoId_DiaSemana_HoraInicio_HoraFin",
                table: "AgendasMedico",
                columns: new[] { "MedicoId", "DiaSemana", "HoraInicio", "HoraFin" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExcepcionesAgenda_MedicoId",
                table: "ExcepcionesAgenda",
                column: "MedicoId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicoObraSocial_ObrasSocialesAceptadasId",
                table: "MedicoObraSocial",
                column: "ObrasSocialesAceptadasId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicos_Dni",
                table: "Medicos",
                column: "Dni",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medicos_Matricula",
                table: "Medicos",
                column: "Matricula",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ObrasSociales_Nombre",
                table: "ObrasSociales",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_Dni",
                table: "Pacientes",
                column: "Dni",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_ObraSocialId",
                table: "Pacientes",
                column: "ObraSocialId");

            migrationBuilder.CreateIndex(
                name: "IX_TiposActividad_NombreTipo",
                table: "TiposActividad",
                column: "NombreTipo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_MedicoId",
                table: "Turnos",
                column: "MedicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_PacienteId",
                table: "Turnos",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioMedico_UsuarioId",
                table: "UsuarioMedico",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_NombreUsuario",
                table: "Usuarios",
                column: "NombreUsuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Usuario_MedicoUnico",
                table: "Usuarios",
                column: "MedicoId",
                unique: true,
                filter: "[Rol] = 2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActividadesMedicas");

            migrationBuilder.DropTable(
                name: "AgendasMedico");

            migrationBuilder.DropTable(
                name: "ExcepcionesAgenda");

            migrationBuilder.DropTable(
                name: "MedicoObraSocial");

            migrationBuilder.DropTable(
                name: "UsuarioMedico");

            migrationBuilder.DropTable(
                name: "TiposActividad");

            migrationBuilder.DropTable(
                name: "Turnos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Pacientes");

            migrationBuilder.DropTable(
                name: "Medicos");

            migrationBuilder.DropTable(
                name: "ObrasSociales");
        }
    }
}
