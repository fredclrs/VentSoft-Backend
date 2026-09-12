using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CompraRepository : Repository<Compra>, ICompraRepository
    {
        public CompraRepository(AppDbContext context) : base(context) { }

        public async Task<Compra?> GetByIdWithDetallesAsync(int id)
        {
            return await _context.Compras
                .Include(x => x.Detalles)
                    .ThenInclude(d => d.Articulo)
                .Include(x => x.Proveedor)
                .Include(x => x.Usuario)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Compra>> GetByProveedorAsync(int idProveedor)
        {
            return await _context.Compras
                .Include(x => x.Detalles)
                .Where(x => x.IdProveedor == idProveedor)
                .OrderByDescending(x => x.Fecha)
                .ToListAsync();
        }

        public async Task<List<DetalleCompra>> GetLotesPorVencerAsync(int? diasAnticipacion, int? idArticulo)
        {
            var query = _context.DetalleCompras
                .Include(x => x.Articulo)
                .Include(x => x.Compra)
                .Where(x => x.Lote != null || x.FechaVencimiento != null);

            if (idArticulo.HasValue)
                query = query.Where(x => x.IdArticulo == idArticulo.Value);

            if (diasAnticipacion.HasValue)
            {
                var limite = DateTime.Today.AddDays(diasAnticipacion.Value);
                query = query.Where(x => x.FechaVencimiento != null && x.FechaVencimiento <= limite);
            }

            return await query
                .OrderBy(x => x.FechaVencimiento ?? DateTime.MaxValue)
                .ToListAsync();
        }
    }
}
