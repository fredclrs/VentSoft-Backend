using Domain.Dtos;
using MediatR;

namespace Application.UseCase.PagoOperation.Command.RegistrarPago
{
    public class RegistrarPagoCommand : IRequest<BaseResponse<PagoDto>>
    {
        public RegistrarPagoDto PagoDto { get; set; } = null!;
    }
}
