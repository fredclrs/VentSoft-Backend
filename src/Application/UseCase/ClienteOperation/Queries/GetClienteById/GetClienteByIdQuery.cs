using Domain.Dtos;
using MediatR;

namespace Application.UseCase.ClienteOperation.Queries.GetClienteById
{
    public class GetClienteByIdQuery : IRequest<BaseResponse<ClienteDto>>
    {
        public int Id { get; set; }
    }
}
