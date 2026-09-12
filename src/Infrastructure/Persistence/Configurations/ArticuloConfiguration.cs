using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ArticuloConfiguration : IEntityTypeConfiguration<Articulo>
    {
        public void Configure(EntityTypeBuilder<Articulo> builder)
        {
            builder.ToTable("Articulo");
            builder.ConfigureAudit();

            builder.Property(x => x.Codigo).HasMaxLength(40).IsRequired();
            builder.Property(x => x.Descripcion).HasMaxLength(100);
            builder.Property(x => x.Tamano).HasMaxLength(20).IsRequired();
            builder.Property(x => x.UnidadMedida).HasMaxLength(20);
            builder.Property(x => x.Imagen).HasMaxLength(30);

            builder.HasOne(x => x.Familia)
                .WithMany(x => x.Articulos)
                .HasForeignKey(x => x.IdFamilia)
                .OnDelete(DeleteBehavior.Cascade);

            // Promoción opcional: si se borra la promoción, el artículo no se borra,
            // solo pierde la referencia.
            builder.HasOne(x => x.Promocion)
                .WithMany(x => x.Articulos)
                .HasForeignKey(x => x.IdPromocion)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
