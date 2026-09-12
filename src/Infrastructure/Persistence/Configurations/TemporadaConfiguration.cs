using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class TemporadaConfiguration : IEntityTypeConfiguration<Temporada>
    {
        public void Configure(EntityTypeBuilder<Temporada> builder)
        {
            builder.ToTable("Temporada");
            builder.ConfigureAudit();

            builder.Property(x => x.Nombre)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.MesInicio).IsRequired();
            builder.Property(x => x.MesFin).IsRequired();
        }
    }
}
