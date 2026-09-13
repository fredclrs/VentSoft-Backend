using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class FormaDePagoConfiguration : IEntityTypeConfiguration<FormaDePago>
    {
        public void Configure(EntityTypeBuilder<FormaDePago> builder)
        {
            builder.ToTable("FormaDePago");
            builder.ConfigureAudit();

            builder.Property(x => x.Nombre)
                .HasColumnName("FormaPago")
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(x => x.EsEfectivo)
                .IsRequired();
        }
    }
}
