using Domain.Dtos;
using MediatR;

namespace Application.UseCase.CobroOperation.Queries.GetDeudaCliente
{
    public class GetDeudaClienteQuery : IRequest<BaseResponse<ClienteDeudaDto>>
    {
        public int IdCliente { get; set; }
    }
}
