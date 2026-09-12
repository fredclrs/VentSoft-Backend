
using Domain.Dtos;
using MediatR;

namespace Application.UseCase.UserOperation.Queries.SearchUsers
{
    public class SearchUserQuery : IRequest<BaseResponse<List<UsuarioDto>>>
    {
        public string? Nombre { get; set; }
        public string? DocumentoIdentidad { get; set; }
        public string? Zona { get; set; }
    }
}
