using Domain.Dtos;
using MediatR;

namespace Application.UseCase.FormaDePagoOperation.Command.AddFormaDePago
{
    public class AddFormaDePagoCommand : IRequest<BaseResponse<FormaDePagoDto>>
    {
        public FormaDePagoDto FormaDePagoDto { get; set; } = null!;
    }
}
