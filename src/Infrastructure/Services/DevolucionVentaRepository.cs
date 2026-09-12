using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DevolucionVentaRepository : Repository<DevolucionVenta>, IDevolucionVentaRepository
    {
        public DevolucionVentaRepository(AppDbContext context) : base(context) { }

        public async Task<List<DevolucionVenta>> GetByVentaAsync(int idVenta)
        {
            return await _context.DevolucionesVenta
                .Include(x => x.Detalles)
                .Include(x => x.ArticulosCambio)
                .Where(x => x.IdVenta == idVenta)
                .OrderBy(x => x.Id)
                .ToListAsync();
        }

        public async Task<List<DevolucionVenta>> GetByClienteAsync(int idCliente)
        {
            return await _context.DevolucionesVenta
                .Include(x => x.Detalles)
                .Include(x => x.ArticulosCambio)
                .Where(x => x.IdCliente == idCliente)
                .OrderByDescending(x => x.Fecha)
                .ToListAsync();
        }

        public async Task<List<DevolucionVenta>> GetDelDiaAsync(DateTime fecha)
        {
            return await _context.DevolucionesVenta
                .Include(x => x.Detalles)
                .Include(x => x.ArticulosCambio)
                .Where(x => x.Estado == "AC" && x.Fecha.Date == fecha.Date)
                .OrderBy(x => x.Id)
                .ToListAsync();
        }

        public async Task<DevolucionVenta?> GetByIdWithDetallesAsync(int id)
        {
            return await _context.DevolucionesVenta
                .Include(x => x.Detalles)
                    .ThenInclude(d => d.Articulo)
                .Include(x => x.ArticulosCambio)
                    .ThenInclude(a => a.Articulo)
                .Include(x => x.Venta)
                .Include(x => x.Cliente)
                .Include(x => x.Usuario)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
