using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ConfiguracionEmpresaConfiguration : IEntityTypeConfiguration<ConfiguracionEmpresa>
    {
        public void Configure(EntityTypeBuilder<ConfiguracionEmpresa> builder)
        {
            builder.ToTable("ConfiguracionEmpresa");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Nombre)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Moneda)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(x => x.PermiteVentaACredito)
                .IsRequired();

            builder.Property(x => x.PermiteCompraACredito)
                .IsRequired();
        }
    }
}
