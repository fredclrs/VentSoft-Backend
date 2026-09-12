
using Application.Interfaces;
using Domain.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.UserOperation.Queries.SearchUsers
{
    public class SearchUserQueryHandler : IRequestHandler<SearchUserQuery, BaseResponse<List<UsuarioDto>>>
    {
        private readonly IUsuarioQueryService _usuarioQueryService;
        private readonly ILogger<SearchUserQueryHandler> _logger;
        public SearchUserQueryHandler(IUsuarioQueryService usuarioQueryService, ILogger<SearchUserQueryHandler> logger) {
            _usuarioQueryService = usuarioQueryService;
            _logger = logger;
        }
        public async Task<BaseResponse<List<UsuarioDto>>> Handle(SearchUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var serachUsuario = await _usuarioQueryService.SearchUsersAsync(request.Nombre, request.DocumentoIdentidad, request.Zona);

                return BaseResponse<List<UsuarioDto>>.SuccessResponse(
                    serachUsuario,
                    "Usuarios encontrados correctamente."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar");
                return BaseResponse<List<UsuarioDto>>.FailureResponse("Ocurrió un error al Buscar");
            }
        }
    }
}
