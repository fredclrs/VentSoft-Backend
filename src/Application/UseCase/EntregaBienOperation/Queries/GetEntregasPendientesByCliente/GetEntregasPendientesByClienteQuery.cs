using Domain.Dtos;
using MediatR;

namespace Application.UseCase.EntregaBienOperation.Queries.GetEntregasPendientesByCliente
{
    public class GetEntregasPendientesByClienteQuery : IRequest<BaseResponse<List<EntregaBienDto>>>
    {
        public int IdCliente { get; set; }
    }
}
