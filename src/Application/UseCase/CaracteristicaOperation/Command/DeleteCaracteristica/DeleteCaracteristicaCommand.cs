using Domain.Dtos;
using MediatR;

namespace Application.UseCase.CaracteristicaOperation.Command.DeleteCaracteristica
{
    public class DeleteCaracteristicaCommand : IRequest<BaseResponse<CaracteristicaDto>>
    {
        public int Id { get; set; }
    }
}
