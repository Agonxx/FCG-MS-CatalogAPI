using CatalogAPI.Domain.Entities;

namespace CatalogAPI.Domain.Interfaces.Services
{
    public interface IJogoService
    {
        Task<Jogo> GetByIdAsync(int id);
        Task<List<Jogo>> GetAllAsync();
        Task<bool> CreateAsync(Jogo jogoObj);
        Task<bool> UpdateAsync(Jogo jogoObj);
        Task<bool> DeleteByIdAsync(int id);
    }
}
