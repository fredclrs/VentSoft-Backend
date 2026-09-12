using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class FamiliaConfiguration : IEntityTypeConfiguration<Familia>
    {
        public void Configure(EntityTypeBuilder<Familia> builder)
        {
            builder.ToTable("Familia");
            builder.ConfigureAudit();

            builder.Property(x => x.NombreFamilia).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Descripcion).HasMaxLength(150);
        }
    }
}
