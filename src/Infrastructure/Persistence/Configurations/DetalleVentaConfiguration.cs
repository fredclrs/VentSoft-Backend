using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class DetalleVentaConfiguration : IEntityTypeConfiguration<DetalleVenta>
    {
        public void Configure(EntityTypeBuilder<DetalleVenta> builder)
        {
            builder.ToTable("DetalleVenta");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Venta)
                .WithMany(x => x.Detalles)
                .HasForeignKey(x => x.IdVenta)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Articulo)
                .WithMany(x => x.DetalleVentas)
                .HasForeignKey(x => x.IdArticulo)
                .OnDelete(DeleteBehavior.Restrict); // NO ACTION en la BD: no se debe poder borrar un Articulo con historial
        }
    }
}
