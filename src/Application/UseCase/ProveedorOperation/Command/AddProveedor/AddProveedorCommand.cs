using Domain.Dtos;
using MediatR;

namespace Application.UseCase.ProveedorOperation.Command.AddProveedor
{
    public class AddProveedorCommand : IRequest<BaseResponse<ProveedorDto>>
    {
        public ProveedorDto ProveedorDto { get; set; } = null!;
    }
}
