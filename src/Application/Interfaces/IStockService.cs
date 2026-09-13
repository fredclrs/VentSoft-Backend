
namespace Application.Interfaces
{
    /// <summary>
    /// No hay una única tabla de "movimientos de inventario": el stock disponible se calcula
    /// sumando/restando de varias tablas para un mismo Articulo — lo comprado (DetalleCompra),
    /// lo vendido (DetalleVenta), lo devuelto/entregado en cambio (Devolucion) y las correcciones
    /// manuales (AjusteStock: rotura, vencimiento, robo, conteo real distinto).
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
