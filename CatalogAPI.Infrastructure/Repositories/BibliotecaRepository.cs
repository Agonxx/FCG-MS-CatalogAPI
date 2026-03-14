using CatalogAPI.Domain.DTOs;
using CatalogAPI.Domain.Entities;
using CatalogAPI.Domain.Interfaces.Repositories;
using CatalogAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Infrastructure.Repositories
{
    public class BibliotecaRepository : IBibliotecaRepository
    {
        private readonly CatalogDbContext _db;
        private readonly InfoToken _infoToken;

        public BibliotecaRepository(CatalogDbContext db, InfoToken infoToken)
        {
            _db = db;
            _infoToken = infoToken;
        }

        public async Task<List<JogoDto>> GetUserGamesById(int id)
        {
            return await (from item in _db.ItensBiblioteca
                          join jogo in _db.Jogos on item.JogoId equals jogo.Id
                          where item.UsuarioId == id
                          select new JogoDto
                          {
                              Id = jogo.Id,
                              UsuarioId = item.UsuarioId,
                              Titulo = jogo.Titulo,
                              Descricao = jogo.Descricao,
                              Preco = jogo.Preco,
                              PrecoPago = item.PrecoPago,
                              AdquiridoEm = item.AdquiridoEm
                          }).ToListAsync();
        }

        public async Task<List<JogoDto>> GetMyGames()
        {
            return await (from item in _db.ItensBiblioteca
                          join jogo in _db.Jogos on item.JogoId equals jogo.Id
                          where item.UsuarioId == _infoToken.Id
                          select new JogoDto
                          {
                              Id = jogo.Id,
                              UsuarioId = _infoToken.Id,
                              NomeComprador = _infoToken.Nome,
                              Titulo = jogo.Titulo,
                              Descricao = jogo.Descricao,
                              Preco = jogo.Preco,
                              PrecoPago = item.PrecoPago,
                              AdquiridoEm = item.AdquiridoEm
                          }).ToListAsync();
        }

        public async Task<bool> BuyGame(ItemBiblioteca item)
        {
            _db.ItensBiblioteca.Add(item);
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> ExistsInLibrary(int usuarioId, int jogoId)
        {
            return await _db.ItensBiblioteca.AnyAsync(x => x.UsuarioId == usuarioId && x.JogoId == jogoId);
        }
    }
}
