using Domain.Dtos;
using MediatR;

namespace Application.UseCase.ArticuloOperation.Queries.GetArticuloById
{
    public class GetArticuloByIdQuery : IRequest<BaseResponse<ArticuloDto>>
    {
        public int Id { get; set; }
    }
}
