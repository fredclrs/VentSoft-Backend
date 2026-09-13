using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Proveedor> Proveedores => Set<Proveedor>();
        public DbSet<FormaDePago> FormasDePago => Set<FormaDePago>();
        public DbSet<Familia> Familias => Set<Familia>();
        public DbSet<Caracteristica> Caracteristicas => Set<Caracteristica>();
        public DbSet<Promocion> Promociones => Set<Promocion>();
        public DbSet<Temporada> Temporadas => Set<Temporada>();
        public DbSet<ConfiguracionEmpresa> ConfiguracionesEmpresa => Set<ConfiguracionEmpresa>();
        public DbSet<Articulo> Articulos => Set<Articulo>();
        public DbSet<ArticuloCaracteristica> ArticuloCaracteristicas => Set<ArticuloCaracteristica>();
        public DbSet<Cobro> Cobros => Set<Cobro>();
        public DbSet<Pago> Pagos => Set<Pago>();
        public DbSet<Compra> Compras => Set<Compra>();
        public DbSet<DetalleCompra> DetalleCompras => Set<DetalleCompra>();
        public DbSet<Venta> Ventas => Set<Venta>();
        public DbSet<DetalleVenta> DetalleVentas => Set<DetalleVenta>();
        public DbSet<DevolucionVenta> DevolucionesVenta => Set<DevolucionVenta>();
        public DbSet<DetalleDevolucionVenta> DetalleDevolucionesVenta => Set<DetalleDevolucionVenta>();
        public DbSet<DetalleCambioVenta> DetalleCambiosVenta => Set<DetalleCambioVenta>();
        public DbSet<TipoBien> TiposBien => Set<TipoBien>();
        public DbSet<EntregaBien> EntregasBien => Set<EntregaBien>();
        public DbSet<Liquidacion> Liquidaciones => Set<Liquidacion>();
        public DbSet<MovimientoCaja> MovimientosCaja => Set<MovimientoCaja>();
        public DbSet<AjusteStock> AjustesStock => Set<AjusteStock>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
