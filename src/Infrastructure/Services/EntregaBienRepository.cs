using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class EntregaBienRepository : Repository<EntregaBien>, IEntregaBienRepository
    {
        public EntregaBienRepository(AppDbContext context) : base(context) { }

        public async Task<List<EntregaBien>> GetPendientesByClienteAsync(int idCliente)
        {
            return await _context.EntregasBien
                .Include(x => x.TipoBien)
                .Where(x => x.IdCliente == idCliente && x.Estado == "AC" && x.IdLiquidacion == null)
                .OrderBy(x => x.Fecha)
                .ToListAsync();
        }

        public async Task<List<EntregaBien>> GetByClienteAsync(int idCliente)
        {
            return await _context.EntregasBien
                .Include(x => x.TipoBien)
                .Where(x => x.IdCliente == idCliente)
                .OrderByDescending(x => x.Fecha)
                .ToListAsync();
        }

        public async Task<EntregaBien?> GetByIdWithRelacionesAsync(int id)
        {
            return await _context.EntregasBien
                .Include(x => x.TipoBien)
                .Include(x => x.Cliente)
                .Include(x => x.Usuario)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
