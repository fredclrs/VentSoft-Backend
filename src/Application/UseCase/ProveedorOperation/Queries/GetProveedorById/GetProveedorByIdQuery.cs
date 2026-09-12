using Domain.Dtos;
using MediatR;

namespace Application.UseCase.ProveedorOperation.Queries.GetProveedorById
{
    public class GetProveedorByIdQuery : IRequest<BaseResponse<ProveedorDto>>
    {
        public int Id { get; set; }
    }
}
