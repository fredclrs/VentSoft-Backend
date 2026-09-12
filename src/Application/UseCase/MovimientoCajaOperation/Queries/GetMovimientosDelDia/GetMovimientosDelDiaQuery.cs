using Domain.Dtos;
using MediatR;

namespace Application.UseCase.MovimientoCajaOperation.Queries.GetMovimientosDelDia
{
    public class GetMovimientosDelDiaQuery : IRequest<BaseResponse<List<MovimientoCajaDto>>>
    {
        public DateTime Fecha { get; set; }
    }
}
