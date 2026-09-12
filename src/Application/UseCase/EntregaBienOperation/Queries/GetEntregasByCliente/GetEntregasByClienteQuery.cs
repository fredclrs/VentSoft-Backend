using Domain.Dtos;
using MediatR;

namespace Application.UseCase.EntregaBienOperation.Queries.GetEntregasByCliente
{
    public class GetEntregasByClienteQuery : IRequest<BaseResponse<List<EntregaBienDto>>>
    {
        public int IdCliente { get; set; }
    }
}
