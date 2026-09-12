using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuario");
            builder.ConfigureAudit(fechaActualizadoColumn: "FechaActilizado");

            builder.Property(x => x.Nombre).HasMaxLength(50).IsRequired();
            builder.Property(x => x.DocumentoIdentidad).HasMaxLength(20).IsRequired();
            builder.Property(x => x.Nit).HasMaxLength(50);
            builder.Property(x => x.Direccion).HasMaxLength(100);
            builder.Property(x => x.Zona).HasMaxLength(100);
            builder.Property(x => x.Correo).HasMaxLength(30);
            builder.Property(x => x.Nota).HasMaxLength(200);

            builder.Property(x => x.NombreUsuario)
                .HasColumnName("NombreUSuario")
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(x => x.NombreUsuario).IsUnique();

            builder.Property(x => x.EsAdministrador).IsRequired();
            builder.Property(x => x.Permisos).HasMaxLength(500).IsRequired();

            // Ampliada de varchar(20) a varchar(100) vía ALTER_ContrasenaHash.sql: un hash
            // SHA-256 en Base64 ocupa 44 caracteres y no entraba en la columna original.
            builder.Property(x => x.Contrasena).HasMaxLength(100).IsRequired();
            builder.Property(x => x.ConfirmarContrasena).HasMaxLength(100).IsRequired();
        }
    }
}
