using Application.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Services
{
    /// <summary>Ver IStockTransaction. Envuelve una transacción real de SQL Server: cada
    /// BloquearArticuloAsync toma un application lock (sp_getapplock) exclusivo, con alcance
    /// "Transaction" — se libera solo al hacer commit o rollback de esta transacción, nunca
    /// antes, así que mientras esté abierta ninguna otra operación puede tomar el mismo lock
    /// para el mismo artículo (queda esperando hasta que ésta termine).</summary>
    public class StockTransaction : IStockTransaction
    {
        private readonly AppDbContext _context;
        private readonly IDbContextTransaction _transaction;
        private bool _confirmada;

        public StockTransaction(AppDbContext context, IDbContextTransaction transaction)
        {
            _context = context;
            _transaction = transaction;
        }

        public async Task BloquearArticuloAsync(int idArticulo)
        {
            var recurso = $"Articulo_{idArticulo}";

            // sp_getapplock devuelve un código negativo si no logra tomar el lock (timeout,
            // deadlock, etc.) sin lanzar una excepción por sí solo — el THROW de acá lo
            // convierte en un error real que sí frena la operación en vez de seguir de largo
            // como si el lock se hubiera tomado.
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
                DECLARE @resultado INT;
                EXEC @resultado = sp_getapplock
                    @Resource = {recurso},
                    @LockMode = 'Exclusive',
                    @LockOwner = 'Transaction',
                    @LockTimeout = 30000;
                IF @resultado < 0
                    THROW 51000, 'No se pudo bloquear el artículo para la operación (otra venta lo está usando).', 1;");
        }

        public async Task ConfirmarAsync()
        {
            await _transaction.CommitAsync();
            _confirmada = true;
        }

        public async ValueTask DisposeAsync()
        {
            if (!_confirmada)
                await _transaction.RollbackAsync();

            await _transaction.DisposeAsync();
        }
    }
}
