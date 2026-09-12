
using Application.Interfaces;
using Application.UseCase.UserOperation.Command.AddUser;
using AutoFixture;
using Domain.Dtos;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestVentSoft.Tests.UseCase.UseOperation.Command.AddUser
{
    public class AddUserCommandHandlerTests
    {
        private readonly Mock<IUsuarioCommandService> _usuarioCommandService;
        private readonly Mock<ILogger<AddUserCommandHandler>> _logger;
        private readonly AddUserCommandHandler _handler;
        private readonly CancellationToken _cancellationToken;
        private readonly Fixture _fixture;
        public AddUserCommandHandlerTests() { 
            _usuarioCommandService = new Mock<IUsuarioCommandService>();
            _logger = new Mock<ILogger<AddUserCommandHandler>>();
            _cancellationToken = CancellationToken.None;
            _handler = new AddUserCommandHandler(_usuarioCommandService.Object, _logger.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task ReturnSuccess()
        {
            // Arrange
            var usuarioDto = _fixture.Create<UsuarioDto>();
            var command = new AddUserCommand { UsuarioDto = usuarioDto };

            _usuarioCommandService
                .Setup(x => x.AddUserAsync(usuarioDto))
                .ReturnsAsync(usuarioDto);

            // Act
            var result = await _handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Usuario agregado correctamente.", result.Message);
            Assert.Equal(usuarioDto, result.Data);
        }

        [Fact]
        public async Task ReturnFailure()
        {
            // Arrange
            var usuarioDto = _fixture.Create<UsuarioDto>();
            var command = new AddUserCommand { UsuarioDto = usuarioDto };

            _usuarioCommandService
                .Setup(x => x.AddUserAsync(usuarioDto))
                .ThrowsAsync(new Exception("Error"));

            // Act
            var result = await _handler.Handle(command, _cancellationToken);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Ocurrió un error al agregar el usuario.", result.Message);
            Assert.Null(result.Data);           
        }

    }
}
