using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class EntregaBienConfiguration : IEntityTypeConfiguration<EntregaBien>
    {
        public void Configure(EntityTypeBuilder<EntregaBien> builder)
        {
            builder.ToTable("EntregaBien");
            builder.ConfigureAudit();

            builder.Property(x => x.NumeroBoleta).HasMaxLength(30);
            builder.Property(x => x.Nota).HasMaxLength(150);

            builder.HasOne(x => x.Cliente)
                .WithMany(x => x.EntregasBien)
                .HasForeignKey(x => x.IdCliente)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Usuario)
                .WithMany()
                .HasForeignKey(x => x.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.TipoBien)
                .WithMany(x => x.Entregas)
                .HasForeignKey(x => x.IdTipoBien)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Liquidacion)
                .WithMany(x => x.Entregas)
                .HasForeignKey(x => x.IdLiquidacion)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
