using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ArticuloRepository : Repository<Articulo>, IArticuloRepository
    {
        public ArticuloRepository(AppDbContext context) : base(context) { }

        public async Task<Articulo?> GetByCodigoAsync(string codigo)
        {
            return await _context.Articulos
                .FirstOrDefaultAsync(x => x.Codigo == codigo);
        }

        public async Task<Articulo?> GetByIdWithCaracteristicasAsync(int id)
        {
            return await _context.Articulos
                .Include(x => x.Familia)
                .Include(x => x.Promocion)
                .Include(x => x.Caracteristicas)
                    .ThenInclude(c => c.Caracteristica)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Articulo>> GetAllWithCaracteristicasAsync()
        {
            return await _context.Articulos
                .Include(x => x.Caracteristicas)
                    .ThenInclude(c => c.Caracteristica)
                .ToListAsync();
        }
    }
}
