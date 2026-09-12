using Domain.Dtos;
using MediatR;

namespace Application.UseCase.PromocionOperation.Queries.SearchPromocions
{
    public class SearchPromocionQuery : IRequest<BaseResponse<List<PromocionDto>>>
    {
        public string? NombrePromocion { get; set; }
    }
}
