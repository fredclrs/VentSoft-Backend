using Domain.Dtos;
using MediatR;

namespace Application.UseCase.PagoOperation.Queries.GetDeudaProveedor
{
    public class GetDeudaProveedorQuery : IRequest<BaseResponse<ProveedorDeudaDto>>
    {
        public int IdProveedor { get; set; }
    }
}
