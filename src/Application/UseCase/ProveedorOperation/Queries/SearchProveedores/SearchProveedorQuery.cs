using Domain.Dtos;
using MediatR;

namespace Application.UseCase.ProveedorOperation.Queries.SearchProveedores
{
    public class SearchProveedorQuery : IRequest<BaseResponse<List<ProveedorDto>>>
    {
        public string? Nombre { get; set; }
        public string? Nit { get; set; }
    }
}
