using Domain.Dtos;
using MediatR;

namespace Application.UseCase.PromocionOperation.Queries.GetPromocionById
{
    public class GetPromocionByIdQuery : IRequest<BaseResponse<PromocionDto>>
    {
        public int Id { get; set; }
    }
}
