using Domain.Dtos;
using MediatR;

namespace Application.UseCase.LiquidacionOperation.Command.RegistrarLiquidacion
{
    public class RegistrarLiquidacionCommand : IRequest<BaseResponse<LiquidacionDto>>
    {
        public RegistrarLiquidacionDto LiquidacionDto { get; set; } = null!;
    }
}
