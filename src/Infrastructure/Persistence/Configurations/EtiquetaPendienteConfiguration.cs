using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class EtiquetaPendienteConfiguration : IEntityTypeConfiguration<EtiquetaPendiente>
    {
        public void Configure(EntityTypeBuilder<EtiquetaPendiente> builder)
        {
            builder.ToTable("EtiquetaPendiente");

            builder.HasOne(x => x.Articulo)
                .WithMany()
                .HasForeignKey(x => x.IdArticulo)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
