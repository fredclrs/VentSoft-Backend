using Domain.Dtos;
using MediatR;

namespace Application.UseCase.VentaOperation.Command.AnularVenta
{
    /// <summary>Anula una venta registrada por error (Estado = "AN"): deja de contar para stock,
    /// totales, ganancia y deuda del cliente, pero queda guardada con quién, cuándo y por qué.
    /// Solo si nada la tocó después (cobros, devoluciones, liquidaciones) — en ese caso hay
    /// plata ya aplicada y se usa Devolución.</summary>
    public class AnularVentaCommand : IRequest<BaseResponse<VentaDto>>
    {
        public int IdVenta { get; set; }
        public string Motivo { get; set; } = null!;

        /// <summary>NombreUsuario de quien anula — lo completa el controller desde el token, no
        /// viene del cliente.</summary>
        public string? Usuario { get; set; }
    }
}
