using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGC.Datos.Migrations
{
    /// <inheritdoc />
    public partial class RestriccionesFormato : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_Usuario_NombreUsuario",
                table: "Usuarios",
                sql: "LEN(NombreUsuario) >= 3 AND NombreUsuario NOT LIKE '%[^a-zA-Z0-9_.]%'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Paciente_Apellido",
                table: "Pacientes",
                sql: "Apellido NOT LIKE '%[0-9]%'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Paciente_Dni_NoCero",
                table: "Pacientes",
                sql: "Dni <> REPLICATE('0', LEN(Dni))");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Paciente_Nombre",
                table: "Pacientes",
                sql: "Nombre NOT LIKE '%[0-9]%'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Paciente_Telefono",
                table: "Pacientes",
                sql: "Telefono NOT LIKE '%[^0-9]%' AND LEN(Telefono) BETWEEN 6 AND 15");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Medico_Apellido",
                table: "Medicos",
                sql: "Apellido NOT LIKE '%[0-9]%'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Medico_Dni_NoCero",
                table: "Medicos",
                sql: "Dni <> REPLICATE('0', LEN(Dni))");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Medico_Especialidad",
                table: "Medicos",
                sql: "Especialidad NOT LIKE '%[0-9]%'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Medico_Nombre",
                table: "Medicos",
                sql: "Nombre NOT LIKE '%[0-9]%'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Usuario_NombreUsuario",
                table: "Usuarios");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Paciente_Apellido",
                table: "Pacientes");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Paciente_Dni_NoCero",
                table: "Pacientes");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Paciente_Nombre",
                table: "Pacientes");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Paciente_Telefono",
                table: "Pacientes");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Medico_Apellido",
                table: "Medicos");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Medico_Dni_NoCero",
                table: "Medicos");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Medico_Especialidad",
                table: "Medicos");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Medico_Nombre",
                table: "Medicos");
        }
    }
}
