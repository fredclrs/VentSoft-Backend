using Domain.Dtos;
using MediatR;

namespace Application.UseCase.CaracteristicaOperation.Command.UpdateCaracteristica
{
    public class UpdateCaracteristicaCommand : IRequest<BaseResponse<CaracteristicaDto>>
    {
        public int Id { get; set; }
        public CaracteristicaDto CaracteristicaDto { get; set; } = null!;
    }
}
