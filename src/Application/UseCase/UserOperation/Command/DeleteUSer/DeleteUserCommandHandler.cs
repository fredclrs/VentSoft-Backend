using Application.Interfaces;
using Domain.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.UserOperation.Command.DeleteUSer
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, BaseResponse<UsuarioDto>>
    {
        private readonly IUsuarioCommandService _usuarioCommandService;
        private readonly ILogger<DeleteUserCommandHandler> _logger;
        public DeleteUserCommandHandler(
             IUsuarioCommandService usuarioCommandService,       
             ILogger<DeleteUserCommandHandler> logger) {

            _usuarioCommandService = usuarioCommandService;
            _logger = logger;
        }
        public async Task<BaseResponse<UsuarioDto>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var usuarioEliminado = await _usuarioCommandService.DeleteUserAsync(request.Id);

                return BaseResponse<UsuarioDto>.SuccessResponse(
                    usuarioEliminado,
                    "Usuario Eliminado correctamente."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al Eliminar usuario");
                return BaseResponse<UsuarioDto>.FailureResponse("Ocurrió un error al Eiminar el usuario.");
            }
        }
    }
}
