using Domain.Dtos;
using MediatR;

namespace Application.UseCase.CompraOperation.Command.AnularCompra
{
    /// <summary>Anula una compra registrada por error (Estado = "AN"): deja de contar para stock
    /// y deuda con el proveedor, pero queda guardada con quién, cuándo y por qué. Solo si no
    /// tuvo pagos posteriores y si sacar su mercadería no deja stock negativo.</summary>
    public class AnularCompraCommand : IRequest<BaseResponse<CompraDto>>
    {
        public int IdCompra { get; set; }
        public string Motivo { get; set; } = null!;

        /// <summary>NombreUsuario de quien anula — lo completa el controller desde el token, no
        /// viene del cliente.</summary>
        public string? Usuario { get; set; }
    }
}
