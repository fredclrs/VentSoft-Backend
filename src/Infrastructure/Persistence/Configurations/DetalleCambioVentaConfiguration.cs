using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class DetalleCambioVentaConfiguration : IEntityTypeConfiguration<DetalleCambioVenta>
    {
        public void Configure(EntityTypeBuilder<DetalleCambioVenta> builder)
        {
            builder.ToTable("DetalleCambioVenta");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Devolucion)
                .WithMany(x => x.ArticulosCambio)
                .HasForeignKey(x => x.IdDevolucion)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Articulo)
                .WithMany(x => x.DetalleCambiosVenta)
                .HasForeignKey(x => x.IdArticulo)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
