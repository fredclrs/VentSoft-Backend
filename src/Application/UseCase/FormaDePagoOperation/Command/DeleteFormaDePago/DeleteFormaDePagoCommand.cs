using Domain.Dtos;
using MediatR;

namespace Application.UseCase.FormaDePagoOperation.Command.DeleteFormaDePago
{
    public class DeleteFormaDePagoCommand : IRequest<BaseResponse<FormaDePagoDto>>
    {
        public int Id { get; set; }
    }
}
