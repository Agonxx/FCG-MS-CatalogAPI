using CatalogAPI.Domain.Entities;
using CatalogAPI.Domain.Interfaces.Repositories;
using CatalogAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Infrastructure.Repositories
{
    public class JogoRepository : IJogoRepository
    {
        private readonly CatalogDbContext _db;

        public JogoRepository(CatalogDbContext db)
        {
            _db = db;
        }

        public async Task<List<Jogo>> GetAll()
        {
            return await _db.Jogos.ToListAsync();
        }

        public async Task<Jogo> GetById(int id)
        {
            return await _db.Jogos.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteById(int id)
        {
            var changes = await _db.Jogos.Where(w => w.Id == id).ExecuteDeleteAsync();
            return changes > 0;
        }

        public async Task<bool> Create(Jogo jogoObj)
        {
            _db.Jogos.Add(jogoObj);
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Jogo jogoObj)
        {
            _db.Jogos.Update(jogoObj);
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }
    }
}
