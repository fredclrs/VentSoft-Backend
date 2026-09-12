using Domain.Dtos;
using MediatR;

namespace Application.UseCase.MovimientoCajaOperation.Command.RegistrarMovimientoCaja
{
    public class RegistrarMovimientoCajaCommand : IRequest<BaseResponse<MovimientoCajaDto>>
    {
        public RegistrarMovimientoCajaDto MovimientoDto { get; set; } = null!;
    }
}
