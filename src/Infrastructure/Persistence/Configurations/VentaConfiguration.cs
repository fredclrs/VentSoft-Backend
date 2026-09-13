using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class VentaConfiguration : IEntityTypeConfiguration<Venta>
    {
        public void Configure(EntityTypeBuilder<Venta> builder)
        {
            builder.ToTable("Venta");
            builder.ConfigureAudit();

            builder.Property(x => x.Referencias).HasMaxLength(100);
            builder.Property(x => x.Nota).HasMaxLength(150);

            builder.HasOne(x => x.Cliente)
                .WithMany(x => x.Ventas)
                .HasForeignKey(x => x.IdCliente)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Usuario)
                .WithMany(x => x.Ventas)
                .HasForeignKey(x => x.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            // Promoción opcional: si se borra la promoción, la venta queda intacta
            // (el descuento ya aplicado sigue en DescuentoMonetario/DescuentoPorcentaje).
            builder.HasOne(x => x.Promocion)
                .WithMany(x => x.Ventas)
                .HasForeignKey(x => x.IdPromocion)
                .OnDelete(DeleteBehavior.SetNull);

            // Forma de pago opcional (null en ventas 100% a crédito o de antes de este campo):
            // si se borra la forma de pago, la venta queda intacta, solo pierde esa clasificación.
            builder.HasOne(x => x.FormaDePago)
                .WithMany()
                .HasForeignKey(x => x.IdFormaDePago)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
