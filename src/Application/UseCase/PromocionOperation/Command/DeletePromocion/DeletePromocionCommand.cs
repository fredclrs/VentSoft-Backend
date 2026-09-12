using Domain.Dtos;
using MediatR;

namespace Application.UseCase.PromocionOperation.Command.DeletePromocion
{
    public class DeletePromocionCommand : IRequest<BaseResponse<PromocionDto>>
    {
        public int Id { get; set; }
    }
}
