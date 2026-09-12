
namespace Application.Interfaces
{
    /// <summary>
    /// La BD actual no tiene una tabla de movimientos de inventario: el stock
    /// disponible se calcula como lo comprado (DetalleCompra) menos lo vendido
    /// (DetalleVenta) para un mismo Articulo. Ver notas del proyecto si se
    /// necesita un histórico real de movimientos (entradas/salidas/ajustes).
    /// </summary>
    public interface IStockService
    {
        Task<int> GetStockActualAsync(int idArticulo);
        Task<bool> ValidarStockAsync(int idArticulo, int cantidad);

        /// <summary>Stock de todos los artículos que tuvieron algún movimiento (compra, venta,
        /// devolución o cambio), en un puñado de consultas agrupadas en vez de una por artículo —
        /// para listados donde hace falta el stock de todo el catálogo a la vez (ej: Ventas, que
        /// necesita saber qué artículos ya no tienen stock antes de dejarlos elegir).</summary>
        Task<Dictionary<int, int>> GetStockPorArticuloAsync();

        /// <summary>Abre la transacción de base de datos que usa BloquearArticuloAsync (ver
        /// IStockTransaction) — hay que llamar esto ANTES de validar stock en cualquier
        /// operación que vaya a descontarlo (Venta, Cambio), para que dos operaciones
        /// concurrentes del mismo artículo no puedan pasar el chequeo al mismo tiempo.</summary>
        Task<IStockTransaction> IniciarOperacionDeStockAsync();
    }
}
