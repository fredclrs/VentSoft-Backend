using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class LiquidacionRepository : Repository<Liquidacion>, ILiquidacionRepository
    {
        public LiquidacionRepository(AppDbContext context) : base(context) { }

        public async Task<List<Liquidacion>> GetByClienteAsync(int idCliente)
        {
            return await _context.Liquidaciones
                .Include(x => x.Entregas)
                    .ThenInclude(e => e.TipoBien)
                .Where(x => x.IdCliente == idCliente)
                .OrderByDescending(x => x.Fecha)
                .ToListAsync();
        }

        public async Task<Liquidacion?> GetByIdWithDetallesAsync(int id)
        {
            return await _context.Liquidaciones
                .Include(x => x.Entregas)
                    .ThenInclude(e => e.TipoBien)
                .Include(x => x.Cliente)
                .Include(x => x.Usuario)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
