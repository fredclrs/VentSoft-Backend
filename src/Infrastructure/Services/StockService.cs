using Application.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class StockService : IStockService
    {
        private readonly AppDbContext _context;

        public StockService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetStockActualAsync(int idArticulo)
        {
            var comprado = await _context.DetalleCompras
                .Where(x => x.IdArticulo == idArticulo)
                .SumAsync(x => x.Cantidad);

            var vendido = await _context.DetalleVentas
                .Where(x => x.IdArticulo == idArticulo)
                .SumAsync(x => x.Cantidad);

            // Lo devuelto (y marcado como vendible) vuelve a sumar al stock; lo entregado
            // como parte de un cambio sale de stock igual que una venta normal.
            var devuelto = await _context.DetalleDevolucionesVenta
                .Where(x => x.IdArticulo == idArticulo && x.Vendible)
                .SumAsync(x => x.Cantidad);

            var entregadoEnCambio = await _context.DetalleCambiosVenta
                .Where(x => x.IdArticulo == idArticulo)
                .SumAsync(x => x.Cantidad);

            // Correcciones manuales (rotura, vencimiento, robo, conteo real distinto) — ver
            // AjusteStock. Solo las activas: una dada de baja (cargada por error) no cuenta.
            var ajustadoEntrada = await _context.AjustesStock
                .Where(x => x.IdArticulo == idArticulo && x.Estado == "AC" && x.Tipo == "ENTRADA")
                .SumAsync(x => x.Cantidad);

            var ajustadoSalida = await _context.AjustesStock
                .Where(x => x.IdArticulo == idArticulo && x.Estado == "AC" && x.Tipo == "SALIDA")
                .SumAsync(x => x.Cantidad);

            return comprado - vendido + devuelto - entregadoEnCambio + ajustadoEntrada - ajustadoSalida;
        }

        public async Task<bool> ValidarStockAsync(int idArticulo, int cantidad)
        {
            var stock = await GetStockActualAsync(idArticulo);
            return stock >= cantidad;
        }

        public async Task<Dictionary<int, int>> GetStockPorArticuloAsync()
        {
            var comprado = await _context.DetalleCompras
                .GroupBy(x => x.IdArticulo)
                .Select(g => new { g.Key, Cantidad = g.Sum(x => x.Cantidad) })
                .ToDictionaryAsync(x => x.Key, x => x.Cantidad);

            var vendido = await _context.DetalleVentas
                .GroupBy(x => x.IdArticulo)
                .Select(g => new { g.Key, Cantidad = g.Sum(x => x.Cantidad) })
                .ToDictionaryAsync(x => x.Key, x => x.Cantidad);

            var devuelto = await _context.DetalleDevolucionesVenta
                .Where(x => x.Vendible)
                .GroupBy(x => x.IdArticulo)
                .Select(g => new { g.Key, Cantidad = g.Sum(x => x.Cantidad) })
                .ToDictionaryAsync(x => x.Key, x => x.Cantidad);

            var entregadoEnCambio = await _context.DetalleCambiosVenta
                .GroupBy(x => x.IdArticulo)
                .Select(g => new { g.Key, Cantidad = g.Sum(x => x.Cantidad) })
                .ToDictionaryAsync(x => x.Key, x => x.Cantidad);

            var ajustadoEntrada = await _context.AjustesStock
                .Where(x => x.Estado == "AC" && x.Tipo == "ENTRADA")
                .GroupBy(x => x.IdArticulo)
                .Select(g => new { g.Key, Cantidad = g.Sum(x => x.Cantidad) })
                .ToDictionaryAsync(x => x.Key, x => x.Cantidad);

            var ajustadoSalida = await _context.AjustesStock
                .Where(x => x.Estado == "AC" && x.Tipo == "SALIDA")
                .GroupBy(x => x.IdArticulo)
                .Select(g => new { g.Key, Cantidad = g.Sum(x => x.Cantidad) })
                .ToDictionaryAsync(x => x.Key, x => x.Cantidad);

            var idsArticulos = comprado.Keys
                .Union(vendido.Keys)
                .Union(devuelto.Keys)
                .Union(entregadoEnCambio.Keys)
                .Union(ajustadoEntrada.Keys)
                .Union(ajustadoSalida.Keys);

            var resultado = new Dictionary<int, int>();
            foreach (var id in idsArticulos)
            {
                resultado[id] = comprado.GetValueOrDefault(id) - vendido.GetValueOrDefault(id)
                    + devuelto.GetValueOrDefault(id) - entregadoEnCambio.GetValueOrDefault(id)
                    + ajustadoEntrada.GetValueOrDefault(id) - ajustadoSalida.GetValueOrDefault(id);
            }

            return resultado;
        }

        public async Task<IStockTransaction> IniciarOperacionDeStockAsync()
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            return new StockTransaction(_context, transaction);
        }
    }
}
