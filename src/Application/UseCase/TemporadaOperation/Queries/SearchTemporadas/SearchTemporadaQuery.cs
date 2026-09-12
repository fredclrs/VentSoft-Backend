using Domain.Dtos;
using MediatR;

namespace Application.UseCase.TemporadaOperation.Queries.SearchTemporadas
{
    public class SearchTemporadaQuery : IRequest<BaseResponse<List<TemporadaDto>>>
    {
        public string? Nombre { get; set; }
    }
}
