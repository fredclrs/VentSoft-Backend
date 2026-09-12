using Domain.Dtos;
using MediatR;

namespace Application.UseCase.CompraOperation.Queries.GetCompraById
{
    public class GetCompraByIdQuery : IRequest<BaseResponse<CompraDto>>
    {
        public int Id { get; set; }
    }
}
