using Domain.Dtos;
using MediatR;

namespace Application.UseCase.CaracteristicaOperation.Queries.SearchCaracteristicas
{
    public class SearchCaracteristicaQuery : IRequest<BaseResponse<List<CaracteristicaDto>>>
    {
        public string? NombreCaracteristica { get; set; }
    }
}
