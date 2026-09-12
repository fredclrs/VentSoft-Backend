using Domain.Dtos;
using MediatR;

namespace Application.UseCase.ArticuloOperation.Queries.SearchArticulos
{
    public class SearchArticuloQuery : IRequest<BaseResponse<List<ArticuloDto>>>
    {
        public string? Codigo { get; set; }
        public string? Descripcion { get; set; }
        public int? IdFamilia { get; set; }
    }
}
