using Domain.Dtos;
using MediatR;

namespace Application.UseCase.VentaOperation.Queries.GetVentasByCliente
{
    public class GetVentasByClienteQuery : IRequest<BaseResponse<List<VentaDto>>>
    {
        public int IdCliente { get; set; }
    }
}
