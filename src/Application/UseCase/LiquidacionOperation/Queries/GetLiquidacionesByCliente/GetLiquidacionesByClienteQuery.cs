using Domain.Dtos;
using MediatR;

namespace Application.UseCase.LiquidacionOperation.Queries.GetLiquidacionesByCliente
{
    public class GetLiquidacionesByClienteQuery : IRequest<BaseResponse<List<LiquidacionDto>>>
    {
        public int IdCliente { get; set; }
    }
}
