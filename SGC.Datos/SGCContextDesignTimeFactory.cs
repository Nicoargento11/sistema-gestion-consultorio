using Microsoft.EntityFrameworkCore.Design;

namespace SGC.Datos;

// Usada solo por las herramientas de EF Core (dotnet ef migrations/database
// update) para poder instanciar el DbContext en tiempo de diseno, sin
// depender de que la app (SGC.UI) este armada como startup project.
public class SGCContextDesignTimeFactory : IDesignTimeDbContextFactory<SGCContext>
{
    public SGCContext CreateDbContext(string[] args) => SGCContextFactory.Crear();
}
