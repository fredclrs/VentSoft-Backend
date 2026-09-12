using Domain.Dtos;
using MediatR;

namespace Application.UseCase.CobroOperation.Queries.GetCobrosByCliente
{
    public class GetCobrosByClienteQuery : IRequest<BaseResponse<List<CobroDto>>>
    {
        public int IdCliente { get; set; }
    }
}
