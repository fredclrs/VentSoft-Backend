using Application.Interfaces;
using Domain.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.AuthOperation.Command.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, BaseResponse<LoginResponseDto>>
    {
        private readonly IAuthService _authService;
        private readonly ILogger<LoginCommandHandler> _logger;

        public LoginCommandHandler(IAuthService authService, ILogger<LoginCommandHandler> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        public async Task<BaseResponse<LoginResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var usuario = await _authService.ValidateUserAsync(
                    request.LoginRequestDto!.NombreUsuario,
                    request.LoginRequestDto.Contrasena);

                if (usuario == null)
                    return BaseResponse<LoginResponseDto>.FailureResponse("Usuario o contraseña incorrectos.");

                var (token, expiracion) = _authService.GenerateToken(usuario);

                var response = new LoginResponseDto
                {
                    Token = token,
                    Expiracion = expiracion,
                    Usuario = usuario
                };

                return BaseResponse<LoginResponseDto>.SuccessResponse(response, "Inicio de sesión correcto.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al iniciar sesión");
                return BaseResponse<LoginResponseDto>.FailureResponse("Ocurrió un error al iniciar sesión.");
            }
        }
    }
}
