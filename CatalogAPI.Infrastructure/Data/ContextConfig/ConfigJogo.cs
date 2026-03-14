using CatalogAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogAPI.Infrastructure.Data.ContextConfig
{
    public class ConfigJogo : IEntityTypeConfiguration<Jogo>
    {
        public void Configure(EntityTypeBuilder<Jogo> builder)
        {
            builder.HasData(new[]
            {
                new Jogo {
                    Id = 1,
                    Titulo = "Elden Ring",
                    Descricao = "RPG",
                    Preco = 249.90m,
                    CadastradoEm = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Jogo {
                    Id = 2,
                    Titulo = "EA FC 26",
                    Descricao = "Esporte",
                    Preco = 249.90m,
                    CadastradoEm = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
            });
        }
    }
}
