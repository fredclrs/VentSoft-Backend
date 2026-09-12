using Domain.Dtos;
using MediatR;

namespace Application.UseCase.ArticuloOperation.Queries.GetArticulosStockBajo
{
    /// <summary>Artículos activos cuyo stock actual (compras - ventas) llegó o bajó de su StockMinimo.</summary>
    public class GetArticulosStockBajoQuery : IRequest<BaseResponse<List<ArticuloStockDto>>>
    {
    }
}
