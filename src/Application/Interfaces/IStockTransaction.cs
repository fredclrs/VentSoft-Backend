namespace Application.Interfaces
{
    /// <summary>
    /// Evita que dos ventas/cambios concurrentes del mismo artículo puedan "pasar" el chequeo
    /// de stock al mismo tiempo y terminar vendiendo de más (ej: dos cajeros vendiendo la
    /// última unidad de algo al mismo instante en un pico de ventas). Uso:
    ///
    /// await using var stockTx = await _stockService.IniciarOperacionDeStockAsync();
    /// await stockTx.BloquearArticuloAsync(idArticulo);   // uno por cada artículo involucrado
    /// // ... acá sí, validar stock y armar la venta/cambio: ya no puede haber otra operación
    /// // concurrente sobre el mismo artículo hasta que ésta termine ...
    /// await _repository.SaveChangesAsync();
    /// await stockTx.ConfirmarAsync();                    // recién ahí se liberan los bloqueos
    ///
    /// Si no se llama ConfirmarAsync (por una devolución de error o una excepción), el
    /// "await using" deshace todo al salir del scope — no queda nada a medio guardar.
    /// </summary>
    public interface IStockTransaction : IAsyncDisposable
    {
        Task BloquearArticuloAsync(int idArticulo);
        Task ConfirmarAsync();
    }
}
