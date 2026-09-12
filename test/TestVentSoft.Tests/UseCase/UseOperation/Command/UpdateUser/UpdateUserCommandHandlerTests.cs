
using Application.Interfaces;
using Application.UseCase.UserOperation.Command.DeleteUSer;
using Application.UseCase.UserOperation.Command.UpdateUSer;
using AutoFixture;
using Domain.Dtos;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestVentSoft.Tests.UseCase.UseOperation.Command.UpdateUser
{
    public class UpdateUserCommandHandlerTests
    {
        private readonly Mock<IUsuarioCommandService> _usuarioCommandService;
        private readonly Mock<ILogger<UpdateUserCommandHandler>> _logger;
        private readonly UpdateUserCommandHandler _handler;
        private readonly CancellationToken _cancellationToken;
        private readonly Fixture _fixture;
        public UpdateUserCommandHandlerTests() {
            _usuarioCommandService = new Mock<IUsuarioCommandService>();
            _logger = new Mock<ILogger<UpdateUserCommandHandler>>();
            _cancellationToken = CancellationToken.None;
            _handler = new UpdateUserCommandHandler(_usuarioCommandService.Object, _logger.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task ReturnSuccess()
        {
            // Arrange
            var usuarioDto = _fixture.Create<UsuarioDto>();
            var userId = _fixture.Create<int>();
            var command = new UpdateUserCommand { Id = userId, UsuarioDto = usuarioDto };

            _usuarioCommandService
                .Setup(x => x.UpdateUserAsync(userId, usuarioDto))
                .ReturnsAsync(usuarioDto);

            // Act
            var result = await _handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Usuario Actualizado correctamente.", result.Message);
            Assert.Equal(usuarioDto, result.Data);

            // Verificamos que el servicio fue llamado una sola vez con los parámetros correctos
            _usuarioCommandService.Verify(x => x.UpdateUserAsync(userId, usuarioDto), Times.Once);
        }

        [Fact]
        public async Task ReturnFailure()
        {
            // Arrange
            var usuarioDto = _fixture.Create<UsuarioDto>();
            var userId = _fixture.Create<int>();
            var command = new UpdateUserCommand { Id = userId, UsuarioDto = usuarioDto };

            _usuarioCommandService
                .Setup(x => x.UpdateUserAsync(userId, usuarioDto))
                .ThrowsAsync(new Exception("Error al actualizar"));

            // Act
            var result = await _handler.Handle(command, _cancellationToken);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Ocurrió un error al Actualizar el usuario.", result.Message);
            Assert.Null(result.Data);           
        }
    }
}
