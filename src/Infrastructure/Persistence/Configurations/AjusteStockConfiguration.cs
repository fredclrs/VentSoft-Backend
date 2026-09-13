using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class AjusteStockConfiguration : IEntityTypeConfiguration<AjusteStock>
    {
        public void Configure(EntityTypeBuilder<AjusteStock> builder)
        {
            builder.ToTable("AjusteStock");
            builder.ConfigureAudit();

            builder.Property(x => x.Tipo)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(x => x.Motivo)
                .HasMaxLength(200)
                .IsRequired();

            builder.HasOne(x => x.Articulo)
                .WithMany()
                .HasForeignKey(x => x.IdArticulo)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Usuario)
                .WithMany()
                .HasForeignKey(x => x.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
