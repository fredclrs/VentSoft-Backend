using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class DevolucionVentaConfiguration : IEntityTypeConfiguration<DevolucionVenta>
    {
        public void Configure(EntityTypeBuilder<DevolucionVenta> builder)
        {
            builder.ToTable("DevolucionVenta");
            builder.ConfigureAudit();

            builder.Property(x => x.Motivo).HasMaxLength(150);

            builder.HasOne(x => x.Venta)
                .WithMany(x => x.Devoluciones)
                .HasForeignKey(x => x.IdVenta)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Cliente)
                .WithMany(x => x.Devoluciones)
                .HasForeignKey(x => x.IdCliente)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Usuario)
                .WithMany()
                .HasForeignKey(x => x.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
