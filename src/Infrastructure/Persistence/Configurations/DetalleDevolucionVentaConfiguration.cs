using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class DetalleDevolucionVentaConfiguration : IEntityTypeConfiguration<DetalleDevolucionVenta>
    {
        public void Configure(EntityTypeBuilder<DetalleDevolucionVenta> builder)
        {
            builder.ToTable("DetalleDevolucionVenta");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Devolucion)
                .WithMany(x => x.Detalles)
                .HasForeignKey(x => x.IdDevolucion)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.DetalleVenta)
                .WithMany(x => x.Devoluciones)
                .HasForeignKey(x => x.IdDetalleVenta)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Articulo)
                .WithMany(x => x.DetalleDevolucionesVenta)
                .HasForeignKey(x => x.IdArticulo)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
