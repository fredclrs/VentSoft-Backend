using Domain.Dtos;
using MediatR;

namespace Application.UseCase.FormaDePagoOperation.Command.UpdateFormaDePago
{
    public class UpdateFormaDePagoCommand : IRequest<BaseResponse<FormaDePagoDto>>
    {
        public int Id { get; set; }
        public FormaDePagoDto FormaDePagoDto { get; set; } = null!;
    }
}
