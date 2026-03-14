using CatalogAPI.Domain.Entities;
using CatalogAPI.Infrastructure.Data.ContextConfig;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Infrastructure.Data
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
        {
        }

        public DbSet<Jogo> Jogos { get; set; }
        public DbSet<ItemBiblioteca> ItensBiblioteca { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ConfigJogo());
            modelBuilder.ApplyConfiguration(new ConfigItemBiblioteca());
        }
    }
}
