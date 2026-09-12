using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class CobroConfiguration : IEntityTypeConfiguration<Cobro>
    {
        public void Configure(EntityTypeBuilder<Cobro> builder)
        {
            builder.ToTable("Cobro");
            builder.ConfigureAudit();

            builder.Property(x => x.Recibo).HasMaxLength(30);
            builder.Property(x => x.Nota).HasMaxLength(150);

            builder.HasOne(x => x.Cliente)
                .WithMany(x => x.Cobros)
                .HasForeignKey(x => x.IdCliente)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Usuario)
                .WithMany(x => x.Cobros)
                .HasForeignKey(x => x.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
