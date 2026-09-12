
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ILiquidacionRepository : IRepository<Liquidacion>
{
    /// <summary>Historial de liquidaciones de un cliente, más recientes primero.</summary>
    Task<List<Liquidacion>> GetByClienteAsync(int idCliente);

    Task<Liquidacion?> GetByIdWithDetallesAsync(int id);
}
