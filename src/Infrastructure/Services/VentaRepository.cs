using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class VentaRepository : Repository<Venta>, IVentaRepository
    {
        public VentaRepository(AppDbContext context) : base(context) { }

        public async Task<List<Venta>> GetByClienteAsync(int idCliente)
        {
            return await _context.Ventas
                .Include(x => x.Detalles)
                .Where(x => x.IdCliente == idCliente)
                .OrderByDescending(x => x.Fecha)
                .ToListAsync();
        }

        public async Task<List<Venta>> GetByFechaAsync(DateTime fecha)
        {
            return await _context.Ventas
                .Include(x => x.Detalles)
                .Where(x => x.Fecha.Date == fecha.Date)
                .OrderBy(x => x.Id)
                .ToListAsync();
        }

        public async Task<List<Venta>> GetTodasAsync()
        {
            return await _context.Ventas
                .Include(x => x.Detalles)
                .Where(x => x.Estado == "AC")
                .OrderByDescending(x => x.Fecha)
                .ToListAsync();
        }

        public async Task<Venta?> GetByIdWithDetallesAsync(int id)
        {
            return await _context.Ventas
                .Include(x => x.Detalles)
                    .ThenInclude(d => d.Articulo)
                .Include(x => x.Cliente)
                .Include(x => x.Usuario)
                .Include(x => x.Promocion)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
