using Domain.Dtos;
using MediatR;

namespace Application.UseCase.TipoBienOperation.Command.AddTipoBien
{
    public class AddTipoBienCommand : IRequest<BaseResponse<TipoBienDto>>
    {
        public TipoBienDto TipoBienDto { get; set; } = null!;
    }
}
