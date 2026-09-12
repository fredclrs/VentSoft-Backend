using Domain.Dtos;
using MediatR;

namespace Application.UseCase.PagoOperation.Queries.GetPagosByProveedor
{
    public class GetPagosByProveedorQuery : IRequest<BaseResponse<List<PagoDto>>>
    {
        public int IdProveedor { get; set; }
    }
}
