using CatalogAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogAPI.Infrastructure.Data.ContextConfig
{
    public class ConfigItemBiblioteca : IEntityTypeConfiguration<ItemBiblioteca>
    {
        public void Configure(EntityTypeBuilder<ItemBiblioteca> builder)
        {
            // FK apenas para Jogo — UsuarioId é referência cross-service (sem FK)
            builder.HasOne<Jogo>()
                   .WithMany()
                   .HasForeignKey(h => h.JogoId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
