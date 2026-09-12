using Domain.Dtos;
using MediatR;

namespace Application.UseCase.DevolucionOperation.Queries.GetDevolucionesByCliente
{
    public class GetDevolucionesByClienteQuery : IRequest<BaseResponse<List<DevolucionVentaDto>>>
    {
        public int IdCliente { get; set; }
    }
}
