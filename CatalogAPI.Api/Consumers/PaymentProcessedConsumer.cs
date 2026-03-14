using MassTransit;
using CatalogAPI.Domain.Entities;
using CatalogAPI.Domain.Interfaces.Repositories;
using Shared.Contracts.Events;

namespace CatalogAPI.Api.Consumers
{
    public class PaymentProcessedConsumer : IConsumer<PaymentProcessedEvent>
    {
        private readonly IBibliotecaRepository _repo;
        private readonly ILogger<PaymentProcessedConsumer> _logger;

        public PaymentProcessedConsumer(IBibliotecaRepository repo, ILogger<PaymentProcessedConsumer> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PaymentProcessedEvent> context)
        {
            var ev = context.Message;

            _logger.LogInformation(
                "PaymentProcessedEvent recebido: OrderId={OrderId} | JogoId={JogoId} | Status={Status}",
                ev.OrderId, ev.JogoId, ev.Status);

            if (ev.Status != "Approved")
                return;

            var item = new ItemBiblioteca
            {
                UsuarioId = ev.UserId,
                JogoId = ev.JogoId,
                PrecoPago = ev.Preco,
                AdquiridoEm = DateTime.UtcNow
            };

            await _repo.BuyGame(item);

            _logger.LogInformation(
                "Jogo {JogoId} adicionado à biblioteca do usuário {UserId}",
                ev.JogoId, ev.UserId);
        }
    }
}
