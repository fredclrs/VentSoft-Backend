
using Application.Interfaces;
using Domain.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.UserOperation.Command.UpdateUSer
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, BaseResponse<UsuarioDto>>
    {
        private readonly IUsuarioCommandService _usuarioCommandService;
        private readonly ILogger<UpdateUserCommandHandler> _logger;

        public UpdateUserCommandHandler(IUsuarioCommandService usuarioCommandService, ILogger<UpdateUserCommandHandler> logger) 
        {
            _usuarioCommandService = usuarioCommandService;
            _logger = logger;
        }

        public async Task<BaseResponse<UsuarioDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var usuarioActulizado = await _usuarioCommandService.UpdateUserAsync(request.Id,request.UsuarioDto);

                return BaseResponse<UsuarioDto>.SuccessResponse(
                    usuarioActulizado,
                    "Usuario Actualizado correctamente."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al Actualizar usuario");
                return BaseResponse<UsuarioDto>.FailureResponse("Ocurrió un error al Actualizar el usuario.");
            }
        }
    }
}
