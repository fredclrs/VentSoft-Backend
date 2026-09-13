using Domain.Dtos;
using MediatR;

namespace Application.UseCase.AjusteStockOperation.Queries.GetAjustesByArticulo
{
    public class GetAjustesByArticuloQuery : IRequest<BaseResponse<List<AjusteStockDto>>>
    {
        public int IdArticulo { get; set; }
    }
}
