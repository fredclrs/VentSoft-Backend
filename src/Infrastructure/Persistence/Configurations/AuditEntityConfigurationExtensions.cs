using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configuración común de los campos heredados de AuditEntity, reutilizable
    /// por todas las entidades auditables (evita repetirla en cada Configuration).
    /// Algunas columnas de la BD real tienen typos históricos (FechaActilizado,
    /// FechaBaje) que se mapean explícitamente sin tocar el nombre en C#.
    /// </summary>
    public static class AuditEntityConfigurationExtensions
    {
        public static void ConfigureAudit<T>(
            this EntityTypeBuilder<T> builder,
            string fechaActualizadoColumn = "FechaActualizado",
            string fechaBajaColumn = "FechaBaja")
            where T : AuditEntity
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Estado)
                .HasColumnType("char(2)")
                .IsRequired();

            builder.Property(x => x.UserRegistro).HasMaxLength(30);
            builder.Property(x => x.UserActualizado).HasMaxLength(30);
            builder.Property(x => x.UserBaja).HasMaxLength(30);

            builder.Property(x => x.FechaRegistro);
            builder.Property(x => x.FechaActualizado).HasColumnName(fechaActualizadoColumn);
            builder.Property(x => x.FechaBaja).HasColumnName(fechaBajaColumn);
        }
    }
}
