using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Cliente");
            builder.ConfigureAudit(fechaActualizadoColumn: "FechaActilizado");

            builder.Property(x => x.Nombre).HasMaxLength(50).IsRequired();
            builder.Property(x => x.DocumentoIdentidad).HasMaxLength(20).IsRequired();
            builder.Property(x => x.PersonaContacto).HasMaxLength(50);
            builder.Property(x => x.Direccion).HasMaxLength(100);
            builder.Property(x => x.Zona).HasMaxLength(100);
            builder.Property(x => x.Telefono).HasMaxLength(20);
            builder.Property(x => x.Correo).HasMaxLength(30);
            builder.Property(x => x.Nota).HasMaxLength(100);
        }
    }
}
