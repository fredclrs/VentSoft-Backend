using Domain.Dtos;
using MediatR;

namespace Application.UseCase.VentaOperation.Command.RegistrarVenta
{
    public class RegistrarVentaCommand : IRequest<BaseResponse<VentaDto>>
    {
        public RegistrarVentaDto VentaDto { get; set; } = null!;
    }
}
