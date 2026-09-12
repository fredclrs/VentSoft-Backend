using Domain.Dtos;
using MediatR;

namespace Application.UseCase.CaracteristicaOperation.Queries.GetCaracteristicaById
{
    public class GetCaracteristicaByIdQuery : IRequest<BaseResponse<CaracteristicaDto>>
    {
        public int Id { get; set; }
    }
}
