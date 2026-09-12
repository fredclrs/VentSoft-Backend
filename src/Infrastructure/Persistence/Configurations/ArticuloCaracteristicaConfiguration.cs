using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ArticuloCaracteristicaConfiguration : IEntityTypeConfiguration<ArticuloCaracteristica>
    {
        public void Configure(EntityTypeBuilder<ArticuloCaracteristica> builder)
        {
            builder.ToTable("ArticuloCaracteristica");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Valor).HasMaxLength(100).IsRequired();

            // Un mismo atributo no puede repetirse dos veces para el mismo artículo.
            builder.HasIndex(x => new { x.IdArticulo, x.IdCaracteristica }).IsUnique();

            builder.HasOne(x => x.Articulo)
                .WithMany(x => x.Caracteristicas)
                .HasForeignKey(x => x.IdArticulo)
                .OnDelete(DeleteBehavior.Cascade);

            // No se puede borrar un tipo de característica que ya está en uso;
            // hay que reasignar/borrar los valores primero.
            builder.HasOne(x => x.Caracteristica)
                .WithMany(x => x.Articulos)
                .HasForeignKey(x => x.IdCaracteristica)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
