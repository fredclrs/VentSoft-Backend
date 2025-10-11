using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            string projectPath = Path.Combine(Directory.GetCurrentDirectory(), "..\\WebEvoltis.API");

            // Construir la configuración
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // Obtener cadena de conexión desde appsettings.json
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseMySql(
                connectionString,
                new MySqlServerVersion(new Version(8, 0, 33))
            );

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
