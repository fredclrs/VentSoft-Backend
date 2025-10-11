
using Application.Interfaces;
using Domain.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.UserOperation.Command.AddUser
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, BaseResponse<UsuarioDto>>
    {
        private readonly IUsuarioCommandService _usuarioCommandService;
        private readonly ILogger<AddUserCommandHandler> _logger;

        public AddUserCommandHandler(
            IUsuarioCommandService usuarioCommandService,
            ILogger<AddUserCommandHandler> logger)
        {
            _usuarioCommandService = usuarioCommandService;
            _logger = logger;
        }
        public async Task<BaseResponse<UsuarioDto>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var nuevoUsuario = await _usuarioCommandService.AddUserAsync(request.UsuarioDto);

                return BaseResponse<UsuarioDto>.SuccessResponse(
                    nuevoUsuario,
                    "Usuario agregado correctamente."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar usuario");
                return BaseResponse<UsuarioDto>.FailureResponse("Ocurrió un error al agregar el usuario.");
            }
        }
      
    }
}
