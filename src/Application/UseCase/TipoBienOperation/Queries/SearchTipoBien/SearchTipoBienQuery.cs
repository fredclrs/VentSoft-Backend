using Domain.Dtos;
using MediatR;

namespace Application.UseCase.TipoBienOperation.Queries.SearchTipoBien
{
    public class SearchTipoBienQuery : IRequest<BaseResponse<List<TipoBienDto>>>
    {
        public string? Nombre { get; set; }
    }
}
