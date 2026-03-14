using CatalogAPI.Domain.Entities;

namespace CatalogAPI.Domain.Interfaces.Repositories
{
    public interface IJogoRepository
    {
        Task<Jogo> GetById(int id);
        Task<bool> DeleteById(int id);
        Task<List<Jogo>> GetAll();
        Task<bool> Create(Jogo jogoObj);
        Task<bool> Update(Jogo jogoObj);
    }
}
