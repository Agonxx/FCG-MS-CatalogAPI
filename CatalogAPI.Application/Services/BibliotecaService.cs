using MassTransit;
using CatalogAPI.Domain.DTOs;
using CatalogAPI.Domain.Interfaces.Repositories;
using CatalogAPI.Domain.Interfaces.Services;
using Shared.Contracts.Events;

namespace CatalogAPI.Application.Services
{
    public class BibliotecaService : IBibliotecaService
    {
        private readonly IBibliotecaRepository _repo;
        private readonly IJogoRepository _repoGame;
        private readonly InfoToken _infoToken;
        private readonly IPublishEndpoint _publishEndpoint;

        public BibliotecaService(IBibliotecaRepository repo, IJogoRepository repoGame, InfoToken infoToken, IPublishEndpoint publishEndpoint)
        {
            _repo = repo;
            _repoGame = repoGame;
            _infoToken = infoToken;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<List<JogoDto>> GetUserGamesByIdAsync(int id)
        {
            return await _repo.GetUserGamesById(id);
        }

        public async Task<List<JogoDto>> GetMyGamesAsync()
        {
            return await _repo.GetMyGames();
        }

        public async Task<bool> BuyGameAsync(int gameId)
        {
            var game = await _repoGame.GetById(gameId);

            if (game is null)
                throw new Exception("Jogo não encontrado");

            var existe = await _repo.ExistsInLibrary(_infoToken.Id, gameId);

            if (existe)
                throw new Exception("Você já possui esse jogo em sua biblioteca");

            await _publishEndpoint.Publish(new OrderPlacedEvent
            {
                OrderId = Guid.NewGuid(),
                UserId = _infoToken.Id,
                JogoId = game.Id,
                Titulo = game.Titulo,
                Preco = game.Preco,
                CriadoEm = DateTime.UtcNow
            });

            return true;
        }
    }
}
