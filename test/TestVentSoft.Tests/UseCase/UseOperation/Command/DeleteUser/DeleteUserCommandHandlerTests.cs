using Application.Interfaces;
using Application.UseCase.UserOperation.Command.DeleteUSer;
using AutoFixture;
using Domain.Dtos;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestVentSoft.Tests.UseCase.UseOperation.Command.DeleteUser
{
    public class DeleteUserCommandHandlerTests
    {
        private readonly Mock<IUsuarioCommandService> _usuarioCommandService;
        private readonly Mock<ILogger<DeleteUserCommandHandler>> _logger;
        private readonly DeleteUserCommandHandler _handler;
        private readonly CancellationToken _cancellationToken;
        private readonly Fixture _fixture;

        public DeleteUserCommandHandlerTests() {
            _usuarioCommandService = new Mock<IUsuarioCommandService>();
            _logger = new Mock<ILogger<DeleteUserCommandHandler>>();
            _cancellationToken = CancellationToken.None;
            _handler = new DeleteUserCommandHandler(_usuarioCommandService.Object, _logger.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task ReturnSuccess()
        {
            // Arrange
            var usuarioDto = _fixture.Create<UsuarioDto>();
            var userId = _fixture.Create<int>();
            var command = new DeleteUserCommand { Id = userId };

            _usuarioCommandService
                .Setup(x => x.DeleteUserAsync(userId))
                .ReturnsAsync(usuarioDto);

            // Act
            var result = await _handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Usuario Eliminado correctamente.", result.Message);
            Assert.Equal(usuarioDto, result.Data);

            // Verificar que se llamó al servicio correctamente
            _usuarioCommandService.Verify(x => x.DeleteUserAsync(userId), Times.Once);
        }

        [Fact]
        public async Task ReturnFailure()
        {
            // Arrange
            var userId = _fixture.Create<int>();
            var command = new DeleteUserCommand { Id = userId };

            _usuarioCommandService
                .Setup(x => x.DeleteUserAsync(userId))
                .ThrowsAsync(new Exception("Error al eliminar"));

            // Act
            var result = await _handler.Handle(command, _cancellationToken);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Ocurrió un error al Eiminar el usuario.", result.Message);
            Assert.Null(result.Data);          
        }


    }
}
