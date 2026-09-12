using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class PromocionConfiguration : IEntityTypeConfiguration<Promocion>
    {
        public void Configure(EntityTypeBuilder<Promocion> builder)
        {
            builder.ToTable("Promocion");
            builder.ConfigureAudit();

            builder.Property(x => x.NombrePromocion).HasMaxLength(80).IsRequired();
            builder.Property(x => x.DescuentoMonetario).HasColumnType("money");
            builder.Property(x => x.DescuentoPorcentaje).HasColumnName("DescuentoProcentaje");
            builder.Property(x => x.Descripcion).HasMaxLength(150);
        }
    }
}
