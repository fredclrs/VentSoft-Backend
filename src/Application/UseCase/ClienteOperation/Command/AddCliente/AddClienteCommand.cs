using Domain.Dtos;
using MediatR;

namespace Application.UseCase.ClienteOperation.Command.AddCliente
{
    public class AddClienteCommand : IRequest<BaseResponse<ClienteDto>>
    {
        public ClienteDto ClienteDto { get; set; } = null!;
    }
}
