using Domain.Dtos;
using MediatR;

namespace Application.UseCase.ProveedorOperation.Command.UpdateProveedor
{
    public class UpdateProveedorCommand : IRequest<BaseResponse<ProveedorDto>>
    {
        public int Id { get; set; }
        public ProveedorDto ProveedorDto { get; set; } = null!;
    }
}
