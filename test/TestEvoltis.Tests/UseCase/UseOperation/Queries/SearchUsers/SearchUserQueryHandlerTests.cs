
using Application.Interfaces;
using Application.UseCase.UserOperation.Queries.SearchUsers;
using AutoFixture;
using Domain.Dtos;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestEvoltis.Tests.UseCase.UseOperation.Queries.SearchUsers
{
    public class SearchUserQueryHandlerTests
    {
        private readonly Mock<IUsuarioQueryService> _usuarioQueryService;
        private readonly Mock<ILogger<SearchUserQueryHandler>> _logger;
        private readonly SearchUserQueryHandler _handler;
        private readonly CancellationToken _cancellationToken;
        private readonly Fixture _fixture;

        public SearchUserQueryHandlerTests() {
            _usuarioQueryService = new Mock<IUsuarioQueryService>();
            _logger = new Mock<ILogger<SearchUserQueryHandler>>();
            _cancellationToken = CancellationToken.None;
            _handler = new SearchUserQueryHandler(_usuarioQueryService.Object, _logger.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task ReturnSuccess()
        {
            // Arrange
            var nombre = _fixture.Create<string>();
            var ciudad = _fixture.Create<string>();
            var provincia = _fixture.Create<string>();
            var usuariosEncontrados = _fixture.Create<List<UsuarioDto>>();

            var query = new SearchUserQuery
            {
                Nombre = nombre,
                Ciudad = ciudad,
                Provincia = provincia
            };

            _usuarioQueryService
                .Setup(x => x.SearchUsersAsync(nombre, ciudad, provincia))
                .ReturnsAsync(usuariosEncontrados);

            // Act
            var result = await _handler.Handle(query, _cancellationToken);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Usuarios encontrados correctamente.", result.Message);
            Assert.Equal(usuariosEncontrados, result.Data);

            // Verificamos que el servicio fue llamado una sola vez con los parámetros correctos
            _usuarioQueryService.Verify(
                x => x.SearchUsersAsync(nombre, ciudad, provincia),
                Times.Once
            );
        }

        [Fact]
        public async Task ReturnFailure()
        {
            // Arrange
            var nombre = _fixture.Create<string>();
            var ciudad = _fixture.Create<string>();
            var provincia = _fixture.Create<string>();

            var query = new SearchUserQuery
            {
                Nombre = nombre,
                Ciudad = ciudad,
                Provincia = provincia
            };

            _usuarioQueryService
                .Setup(x => x.SearchUsersAsync(nombre, ciudad, provincia))
                .ThrowsAsync(new Exception("Error al buscar usuarios"));

            // Act
            var result = await _handler.Handle(query, _cancellationToken);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Ocurrió un error al Buscar", result.Message);
            Assert.Null(result.Data);           
        }
    }
}
