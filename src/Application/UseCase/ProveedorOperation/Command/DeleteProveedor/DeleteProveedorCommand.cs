using Domain.Dtos;
using MediatR;

namespace Application.UseCase.ProveedorOperation.Command.DeleteProveedor
{
    public class DeleteProveedorCommand : IRequest<BaseResponse<ProveedorDto>>
    {
        public int Id { get; set; }
    }
}
