using Domain.Dtos;
using MediatR;

namespace Application.UseCase.CaracteristicaOperation.Command.AddCaracteristica
{
    public class AddCaracteristicaCommand : IRequest<BaseResponse<CaracteristicaDto>>
    {
        public CaracteristicaDto CaracteristicaDto { get; set; } = null!;
    }
}
