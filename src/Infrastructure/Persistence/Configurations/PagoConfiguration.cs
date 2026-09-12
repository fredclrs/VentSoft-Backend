using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class PagoConfiguration : IEntityTypeConfiguration<Pago>
    {
        public void Configure(EntityTypeBuilder<Pago> builder)
        {
            builder.ToTable("Pago");
            builder.ConfigureAudit();

            builder.Property(x => x.Recibo).HasMaxLength(30);
            builder.Property(x => x.Nota).HasMaxLength(150);

            builder.HasOne(x => x.Proveedor)
                .WithMany(x => x.Pagos)
                .HasForeignKey(x => x.IdProveedor)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Usuario)
                .WithMany(x => x.Pagos)
                .HasForeignKey(x => x.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
