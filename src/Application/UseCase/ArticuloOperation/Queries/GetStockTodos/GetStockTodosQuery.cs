using Domain.Dtos;
using MediatR;

namespace Application.UseCase.ArticuloOperation.Queries.GetStockTodos
{
    /// <summary>Stock actual de todos los artículos activos, de una sola vez (ver
    /// IStockService.GetStockPorArticuloAsync) — para listados que necesitan saber qué artículos
    /// ya no tienen stock antes de dejarlos elegir (ej: Ventas).</summary>
    public class GetStockTodosQuery : IRequest<BaseResponse<List<ArticuloStockDto>>>
    {
    }
}
