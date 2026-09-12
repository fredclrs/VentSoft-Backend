using Domain.Dtos;
using MediatR;

namespace Application.UseCase.PromocionOperation.Command.AddPromocion
{
    public class AddPromocionCommand : IRequest<BaseResponse<PromocionDto>>
    {
        public PromocionDto PromocionDto { get; set; } = null!;
    }
}
