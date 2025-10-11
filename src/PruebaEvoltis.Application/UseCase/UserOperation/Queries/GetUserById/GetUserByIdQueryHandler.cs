
using Application.Interfaces;
using Domain.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.UserOperation.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, BaseResponse<UsuarioDto>>
    {
        private readonly IUsuarioQueryService _usuarioQueryService;
        private readonly ILogger<GetUserByIdQueryHandler> _logger;
        public GetUserByIdQueryHandler(IUsuarioQueryService usuarioQueryService, ILogger<GetUserByIdQueryHandler> logger) {
            _usuarioQueryService = usuarioQueryService;
            _logger = logger;
        }
        public async Task<BaseResponse<UsuarioDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var usuario = await _usuarioQueryService.GetUserByIdAsync(request.Id);

                return BaseResponse<UsuarioDto>.SuccessResponse(
                    usuario,
                    "Usuario encontrado correctamente."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario");
                return BaseResponse<UsuarioDto>.FailureResponse("Ocurrió un error al Obtener el usuario.");
            }
        }
    }
}
