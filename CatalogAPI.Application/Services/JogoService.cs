using CatalogAPI.Domain.Entities;
using CatalogAPI.Domain.Extensions;
using CatalogAPI.Domain.Interfaces.Repositories;
using CatalogAPI.Domain.Interfaces.Services;

namespace CatalogAPI.Application.Services
{
    public class JogoService : IJogoService
    {
        private readonly IJogoRepository _repo;

        public JogoService(IJogoRepository repo)
        {
            _repo = repo;
        }

        public async Task<Jogo> GetByIdAsync(int id)
        {
            var jogo = await _repo.GetById(id);

            if (jogo is null)
                throw new Exception("Jogo não encontrado");

            return jogo;
        }

        public async Task<List<Jogo>> GetAllAsync()
        {
            return await _repo.GetAll();
        }

        public async Task<bool> CreateAsync(Jogo jogoObj)
        {
            if (jogoObj.Titulo.IsNullOrEmpty())
                throw new Exception("Sem título");

            if (jogoObj.Preco <= 0)
                throw new Exception("Preço não informado");

            return await _repo.Create(jogoObj);
        }

        public async Task<bool> UpdateAsync(Jogo jogoObj)
        {
            if (jogoObj.Id <= 0)
                throw new Exception("Jogo não encontrado");

            if (jogoObj.Titulo.IsNullOrEmpty())
                throw new Exception("Sem título");

            if (jogoObj.Preco <= 0)
                throw new Exception("Preço não informado");

            var existe = await _repo.GetById(jogoObj.Id);

            if (existe is null)
                throw new Exception("Jogo não encontrado");

            return await _repo.Update(jogoObj);
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            if (id <= 0)
                return false;

            return await _repo.DeleteById(id);
        }
    }
}
