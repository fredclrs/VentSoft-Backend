using Domain.Dtos;
using MediatR;

namespace Application.UseCase.FamiliaOperation.Queries.SearchFamilias
{
    public class SearchFamiliaQuery : IRequest<BaseResponse<List<FamiliaDto>>>
    {
        public string? NombreFamilia { get; set; }
    }
}
