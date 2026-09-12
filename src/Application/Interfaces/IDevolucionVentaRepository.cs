
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IDevolucionVentaRepository : IRepository<DevolucionVenta>
{
    /// <summary>Todas las devoluciones/cambios registrados contra una venta puntual (para validar
    /// cuánto de cada renglón ya se devolvió antes).</summary>
    Task<List<DevolucionVenta>> GetByVentaAsync(int idVenta);

    /// <summary>Historial de devoluciones/cambios de un cliente (más recientes primero).</summary>
    Task<List<DevolucionVenta>> GetByClienteAsync(int idCliente);

    /// <summary>Todas las devoluciones/cambios activos de una fecha puntual — para saber cuánto
    /// efectivo entró o salió de la caja por eso ese día (reporte "Ventas del día").</summary>
    Task<List<DevolucionVenta>> GetDelDiaAsync(DateTime fecha);

    Task<DevolucionVenta?> GetByIdWithDetallesAsync(int id);
}
