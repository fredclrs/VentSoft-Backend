using Domain.Dtos;
using MediatR;

namespace Application.UseCase.CompraOperation.Queries.GetComprasByProveedor
{
    public class GetComprasByProveedorQuery : IRequest<BaseResponse<List<CompraDto>>>
    {
        public int IdProveedor { get; set; }
    }
}
