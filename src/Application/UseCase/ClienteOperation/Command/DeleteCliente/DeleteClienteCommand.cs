using Domain.Dtos;
using MediatR;

namespace Application.UseCase.ClienteOperation.Command.DeleteCliente
{
    public class DeleteClienteCommand : IRequest<BaseResponse<ClienteDto>>
    {
        public int Id { get; set; }
    }
}
