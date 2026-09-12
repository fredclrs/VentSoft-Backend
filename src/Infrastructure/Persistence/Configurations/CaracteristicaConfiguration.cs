using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class CaracteristicaConfiguration : IEntityTypeConfiguration<Caracteristica>
    {
        public void Configure(EntityTypeBuilder<Caracteristica> builder)
        {
            builder.ToTable("Caracteristica");
            builder.ConfigureAudit();

            builder.Property(x => x.NombreCaracteristica).HasMaxLength(80).IsRequired();
            builder.Property(x => x.Descripcion).HasMaxLength(150);
        }
    }
}
