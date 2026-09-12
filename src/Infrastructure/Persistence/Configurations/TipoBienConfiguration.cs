using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class TipoBienConfiguration : IEntityTypeConfiguration<TipoBien>
    {
        public void Configure(EntityTypeBuilder<TipoBien> builder)
        {
            builder.ToTable("TipoBien");
            builder.ConfigureAudit();

            builder.Property(x => x.Nombre).HasMaxLength(50).IsRequired();
            builder.Property(x => x.UnidadMedida).HasMaxLength(30).IsRequired();
        }
    }
}
