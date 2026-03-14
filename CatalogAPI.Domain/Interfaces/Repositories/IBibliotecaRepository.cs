using CatalogAPI.Domain.DTOs;
using CatalogAPI.Domain.Entities;

namespace CatalogAPI.Domain.Interfaces.Repositories
{
    public interface IBibliotecaRepository
    {
        Task<List<JogoDto>> GetUserGamesById(int id);
        Task<List<JogoDto>> GetMyGames();
        Task<bool> BuyGame(ItemBiblioteca item);
        Task<bool> ExistsInLibrary(int usuarioId, int jogoId);
    }
}
