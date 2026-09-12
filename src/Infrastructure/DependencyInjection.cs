using Application.Interfaces;
using Application.Interfaces.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Repositorio genérico de CRUD (ver IRepository<T>).
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IArticuloRepository, ArticuloRepository>();
            services.AddScoped<IVentaRepository, VentaRepository>();
            services.AddScoped<ICompraRepository, CompraRepository>();
            services.AddScoped<IDevolucionVentaRepository, DevolucionVentaRepository>();
            services.AddScoped<IEntregaBienRepository, EntregaBienRepository>();
            services.AddScoped<ILiquidacionRepository, LiquidacionRepository>();

            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IStockService, StockService>();

            services.AddScoped<IUsuarioQueryService, UsuarioQueryService>();
            services.AddScoped<IUsuarioCommandService, UsuarioCommandService>();

            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
