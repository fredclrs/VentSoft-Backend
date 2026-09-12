using Domain.Dtos;
using MediatR;

namespace Application.UseCase.FormaDePagoOperation.Queries.SearchFormaDePagos
{
    public class SearchFormaDePagoQuery : IRequest<BaseResponse<List<FormaDePagoDto>>>
    {
        public string? Nombre { get; set; }
    }
}
