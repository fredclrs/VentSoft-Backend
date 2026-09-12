using Domain.Dtos;
using MediatR;

namespace Application.UseCase.VentaOperation.Queries.GetVentasDelDia
{
    public class GetVentasDelDiaQuery : IRequest<BaseResponse<List<VentaDto>>>
    {
        public DateTime Fecha { get; set; }
    }
}
