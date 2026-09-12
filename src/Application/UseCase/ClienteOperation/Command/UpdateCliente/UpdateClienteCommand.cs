using Domain.Dtos;
using MediatR;

namespace Application.UseCase.ClienteOperation.Command.UpdateCliente
{
    public class UpdateClienteCommand : IRequest<BaseResponse<ClienteDto>>
    {
        public int Id { get; set; }
        public ClienteDto ClienteDto { get; set; } = null!;
    }
}
