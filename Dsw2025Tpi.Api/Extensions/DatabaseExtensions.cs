using Microsoft.EntityFrameworkCore;
using Dsw2025Tpi.Data; // Asegúrate de incluir el namespace donde están tus DbContext

namespace Dsw2025Tpi.Api.Extensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddDynamicDatabaseConfiguration(
            this IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            string connectionString;

            if (environment.IsDevelopment())
            {
                // Entorno Local: Usa la cadena de tu appsettings.json o appsettings.Development.json
                connectionString = configuration.GetConnectionString("Dsw2025TpiEntities");
            }
            else
            {
                // Entorno Producción (Docker/Azure): 
                // Lee la variable de entorno configurada en Azure App Service[cite: 14]
                connectionString = Environment.GetEnvironmentVariable("SQLAZURECONNSTR_Dsw2025TpiEntities");
                                   
            }

            // Si por alguna razón no encuentra la cadena, lanzamos un error claro
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("No se encontró la cadena de conexión para el entorno actual.");
            }
            // Inyectamos la cadena elegida a tus dos contextos
            services.AddDbContext<AuthenticateContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddDbContext<Dsw2025TpiContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }
    }
}