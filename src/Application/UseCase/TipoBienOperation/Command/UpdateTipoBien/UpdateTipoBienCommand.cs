using Domain.Dtos;
using MediatR;

namespace Application.UseCase.TipoBienOperation.Command.UpdateTipoBien
{
    public class UpdateTipoBienCommand : IRequest<BaseResponse<TipoBienDto>>
    {
        public int Id { get; set; }
        public TipoBienDto TipoBienDto { get; set; } = null!;
    }
}
