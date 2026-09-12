using Domain.Dtos;
using MediatR;

namespace Application.UseCase.DevolucionOperation.Queries.GetDevolucionesByVenta
{
    public class GetDevolucionesByVentaQuery : IRequest<BaseResponse<List<DevolucionVentaDto>>>
    {
        public int IdVenta { get; set; }
    }
}
