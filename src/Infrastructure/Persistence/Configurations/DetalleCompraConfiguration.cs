using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class DetalleCompraConfiguration : IEntityTypeConfiguration<DetalleCompra>
    {
        public void Configure(EntityTypeBuilder<DetalleCompra> builder)
        {
            builder.ToTable("DetalleCompra");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CostoUnitario).HasColumnType("money");
            builder.Property(x => x.SubTotal).HasColumnName("SubtoTotal").HasColumnType("money");
            builder.Property(x => x.Pagado).HasColumnType("money");
            builder.Property(x => x.Lote).HasMaxLength(30);

            builder.HasOne(x => x.Compra)
                .WithMany(x => x.Detalles)
                .HasForeignKey(x => x.IdCompra)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Articulo)
                .WithMany(x => x.DetalleCompras)
                .HasForeignKey(x => x.IdArticulo)
                .OnDelete(DeleteBehavior.Restrict); // NO ACTION en la BD: no se debe poder borrar un Articulo con historial
        }
    }
}
