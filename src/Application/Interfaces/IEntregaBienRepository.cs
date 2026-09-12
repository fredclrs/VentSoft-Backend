
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IEntregaBienRepository : IRepository<EntregaBien>
{
    /// <summary>Boletas de un cliente que todavía no se incluyeron en ninguna liquidación.</summary>
    Task<List<EntregaBien>> GetPendientesByClienteAsync(int idCliente);

    /// <summary>Historial completo (pendientes y liquidadas) de un cliente, más recientes primero.</summary>
    Task<List<EntregaBien>> GetByClienteAsync(int idCliente);

    Task<EntregaBien?> GetByIdWithRelacionesAsync(int id);
}
