
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IVentaRepository : IRepository<Venta>
{
    Task<List<Venta>> GetByClienteAsync(int idCliente);

    /// <summary>Todas las ventas (de cualquier cliente) hechas en una fecha puntual — para el reporte "Ventas del día".</summary>
    Task<List<Venta>> GetByFechaAsync(DateTime fecha);

    /// <summary>Todas las ventas activas (de cualquier cliente/vendedor) — para los reportes
    /// "Ventas por vendedor" y "Ventas por artículo" (permiso aparte del de Ventas: por eso
    /// no se arma juntando GetByClienteAsync desde el front, se necesita un endpoint propio).</summary>
    Task<List<Venta>> GetTodasAsync();

    Task<Venta?> GetByIdWithDetallesAsync(int id);
}
