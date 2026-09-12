using Domain.Dtos;
using MediatR;

namespace Application.UseCase.TipoBienOperation.Queries.GetTipoBienById
{
    public class GetTipoBienByIdQuery : IRequest<BaseResponse<TipoBienDto>>
    {
        public int Id { get; set; }
    }
}
