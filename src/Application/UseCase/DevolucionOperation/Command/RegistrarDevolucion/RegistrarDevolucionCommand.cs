using Domain.Dtos;
using MediatR;

namespace Application.UseCase.DevolucionOperation.Command.RegistrarDevolucion
{
    public class RegistrarDevolucionCommand : IRequest<BaseResponse<DevolucionVentaDto>>
    {
        public RegistrarDevolucionDto DevolucionDto { get; set; } = null!;
    }
}
