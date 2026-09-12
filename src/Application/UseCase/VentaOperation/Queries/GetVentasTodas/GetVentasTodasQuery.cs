using Domain.Dtos;
using MediatR;

namespace Application.UseCase.VentaOperation.Queries.GetVentasTodas
{
    /// <summary>Todas las ventas activas, de cualquier cliente/vendedor — para los reportes
    /// "Ventas por vendedor" y "Ventas por artículo" (permiso VentasPorVendedor, independiente
    /// del permiso Ventas).</summary>
    public class GetVentasTodasQuery : IRequest<BaseResponse<List<VentaDto>>>
    {
    }
}
