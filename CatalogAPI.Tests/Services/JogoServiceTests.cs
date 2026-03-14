using CatalogAPI.Application.Services;
using CatalogAPI.Domain.Entities;
using CatalogAPI.Domain.Interfaces.Repositories;
using Moq;

namespace CatalogAPI.Tests.Services
{
    public class JogoServiceTests
    {
        private readonly Mock<IJogoRepository> _repoMock;
        private readonly JogoService _service;

        public JogoServiceTests()
        {
            _repoMock = new Mock<IJogoRepository>();
            _service = new JogoService(_repoMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_DeveRetornarJogo_QuandoIdExiste()
        {
            // Arrange
            var jogoEsperado = new Jogo
            {
                Id = 1,
                Titulo = "Elden Ring",
                Descricao = "RPG",
                Preco = 249.90m,
                CadastradoEm = DateTime.UtcNow
            };

            _repoMock.Setup(r => r.GetById(1)).ReturnsAsync(jogoEsperado);

            // Act
            var resultado = await _service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(jogoEsperado.Id, resultado.Id);
            Assert.Equal(jogoEsperado.Titulo, resultado.Titulo);
            Assert.Equal(jogoEsperado.Preco, resultado.Preco);
        }

        [Fact]
        public async Task GetByIdAsync_DeveLancarExcecao_QuandoJogoNaoExiste()
        {
            // Arrange
            _repoMock.Setup(r => r.GetById(999)).ReturnsAsync((Jogo?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _service.GetByIdAsync(999));
            Assert.Equal("Jogo não encontrado", ex.Message);
        }

        [Fact]
        public async Task GetAllAsync_DeveRetornarListaDeJogos()
        {
            // Arrange
            var jogos = new List<Jogo>
            {
                new Jogo { Id = 1, Titulo = "Elden Ring", Descricao = "RPG", Preco = 249.90m },
                new Jogo { Id = 2, Titulo = "EA FC 26", Descricao = "Esporte", Preco = 249.90m }
            };

            _repoMock.Setup(r => r.GetAll()).ReturnsAsync(jogos);

            // Act
            var resultado = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
            Assert.Contains(resultado, j => j.Titulo == "Elden Ring");
            Assert.Contains(resultado, j => j.Titulo == "EA FC 26");
        }

        [Fact]
        public async Task GetAllAsync_DeveRetornarListaVazia_QuandoNaoHaJogos()
        {
            // Arrange
            _repoMock.Setup(r => r.GetAll()).ReturnsAsync(new List<Jogo>());

            // Act
            var resultado = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }

        [Fact]
        public async Task CreateAsync_DeveRetornarTrue_QuandoJogoCriado()
        {
            // Arrange
            var novoJogo = new Jogo
            {
                Titulo = "God of War",
                Descricao = "Acao e Aventura",
                Preco = 199.90m
            };

            _repoMock.Setup(r => r.Create(novoJogo)).ReturnsAsync(true);

            // Act
            var resultado = await _service.CreateAsync(novoJogo);

            // Assert
            Assert.True(resultado);
            _repoMock.Verify(r => r.Create(novoJogo), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_DeveLancarExcecao_QuandoTituloVazio()
        {
            // Arrange
            var jogoSemTitulo = new Jogo { Titulo = "", Preco = 99.90m };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(jogoSemTitulo));
            Assert.Equal("Sem título", ex.Message);
        }

        [Fact]
        public async Task CreateAsync_DeveLancarExcecao_QuandoPrecoZero()
        {
            // Arrange
            var jogoSemPreco = new Jogo { Titulo = "Jogo Teste", Preco = 0 };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _service.CreateAsync(jogoSemPreco));
            Assert.Equal("Preço não informado", ex.Message);
        }

        [Fact]
        public async Task UpdateAsync_DeveRetornarTrue_QuandoJogoAtualizado()
        {
            // Arrange
            var jogo = new Jogo
            {
                Id = 1,
                Titulo = "Elden Ring - GOTY",
                Descricao = "RPG Atualizado",
                Preco = 299.90m
            };

            _repoMock.Setup(r => r.GetById(jogo.Id)).ReturnsAsync(jogo);
            _repoMock.Setup(r => r.Update(jogo)).ReturnsAsync(true);

            // Act
            var resultado = await _service.UpdateAsync(jogo);

            // Assert
            Assert.True(resultado);
            _repoMock.Verify(r => r.Update(jogo), Times.Once);
        }

        [Fact]
        public async Task DeleteByIdAsync_DeveRetornarTrue_QuandoJogoExcluido()
        {
            // Arrange
            _repoMock.Setup(r => r.DeleteById(1)).ReturnsAsync(true);

            // Act
            var resultado = await _service.DeleteByIdAsync(1);

            // Assert
            Assert.True(resultado);
            _repoMock.Verify(r => r.DeleteById(1), Times.Once);
        }

        [Fact]
        public async Task DeleteByIdAsync_DeveRetornarFalse_QuandoIdInvalido()
        {
            // Act
            var resultado = await _service.DeleteByIdAsync(0);

            // Assert
            Assert.False(resultado);
            _repoMock.Verify(r => r.DeleteById(It.IsAny<int>()), Times.Never);
        }
    }
}
