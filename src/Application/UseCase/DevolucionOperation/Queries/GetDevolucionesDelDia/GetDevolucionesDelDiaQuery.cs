using Domain.Dtos;
using MediatR;

namespace Application.UseCase.DevolucionOperation.Queries.GetDevolucionesDelDia
{
    public class GetDevolucionesDelDiaQuery : IRequest<BaseResponse<List<DevolucionVentaDto>>>
    {
        public DateTime Fecha { get; set; }
    }
}
