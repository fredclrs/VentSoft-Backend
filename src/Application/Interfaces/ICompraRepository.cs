
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ICompraRepository : IRepository<Compra>
{
    Task<Compra?> GetByIdWithDetallesAsync(int id);

    Task<List<Compra>> GetByProveedorAsync(int idProveedor);

    /// <summary>Renglones de compra con Lote y/o FechaVencimiento cargados. Si diasAnticipacion
    /// viene, solo los que vencen dentro de esos días (o ya vencidos); si no, todos (sirve para
    /// ver el historial completo de lotes de un artículo puntual, sin importar cuándo vence).
    /// idArticulo es opcional, para acotar a un solo artículo.</summary>
    Task<List<DetalleCompra>> GetLotesPorVencerAsync(int? diasAnticipacion, int? idArticulo);
}
