using Domain.Dtos;
using MediatR;

namespace Application.UseCase.TipoBienOperation.Command.DeleteTipoBien
{
    public class DeleteTipoBienCommand : IRequest<BaseResponse<TipoBienDto>>
    {
        public int Id { get; set; }
    }
}
