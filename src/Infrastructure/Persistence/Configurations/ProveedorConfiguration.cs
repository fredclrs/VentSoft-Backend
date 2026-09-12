using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ProveedorConfiguration : IEntityTypeConfiguration<Proveedor>
    {
        public void Configure(EntityTypeBuilder<Proveedor> builder)
        {
            builder.ToTable("Proveedor");
            builder.ConfigureAudit(fechaBajaColumn: "FechaBaje");

            builder.Property(x => x.Nombre).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Nit).HasMaxLength(30);
            builder.Property(x => x.PersonaContacto).HasMaxLength(50);
            builder.Property(x => x.Direccion).HasMaxLength(100);
            builder.Property(x => x.Zona).HasMaxLength(100);
            builder.Property(x => x.Correo).HasMaxLength(50);
            builder.Property(x => x.Nota).HasMaxLength(100);
        }
    }
}
