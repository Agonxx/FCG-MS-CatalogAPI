using CatalogAPI.Domain.DTOs;

namespace CatalogAPI.Domain.Interfaces.Services
{
    public interface IBibliotecaService
    {
        Task<List<JogoDto>> GetUserGamesByIdAsync(int id);
        Task<List<JogoDto>> GetMyGamesAsync();
        Task<bool> BuyGameAsync(int gameId);
    }
}
