using Domain.Dtos;
using MediatR;

namespace Application.UseCase.ArticuloOperation.Queries.GetArticulosStockBajo
{
    /// <summary>Artículos activos cuyo stock actual (compras - ventas) llegó o bajó de su
    /// StockMinimo — o, si no tienen StockMinimo cargado, que se quedaron en 0 o menos.</summary>
    public class GetArticulosStockBajoQuery : IRequest<BaseResponse<List<ArticuloStockDto>>>
    {
    }
}
