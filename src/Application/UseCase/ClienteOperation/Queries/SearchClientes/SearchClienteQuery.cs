using Domain.Dtos;
using MediatR;

namespace Application.UseCase.ClienteOperation.Queries.SearchClientes
{
    public class SearchClienteQuery : IRequest<BaseResponse<List<ClienteDto>>>
    {
        public string? Nombre { get; set; }
        public string? DocumentoIdentidad { get; set; }
    }
}
