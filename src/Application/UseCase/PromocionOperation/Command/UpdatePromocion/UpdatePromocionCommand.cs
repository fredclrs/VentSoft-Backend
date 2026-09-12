using Domain.Dtos;
using MediatR;

namespace Application.UseCase.PromocionOperation.Command.UpdatePromocion
{
    public class UpdatePromocionCommand : IRequest<BaseResponse<PromocionDto>>
    {
        public int Id { get; set; }
        public PromocionDto PromocionDto { get; set; } = null!;
    }
}
