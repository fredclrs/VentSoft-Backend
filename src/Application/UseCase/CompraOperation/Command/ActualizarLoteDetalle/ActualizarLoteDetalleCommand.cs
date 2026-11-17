using Domain.Dtos;
using MediatR;

namespace Application.UseCase.CompraOperation.Command.ActualizarLoteDetalle
{
    /// <summary>Carga o corrige el Lote y/o la Fecha de vencimiento de un renglón de una compra
    /// YA registrada (ej. no los cargaron al comprar). No toca cantidades, costos, stock ni pagos.
    /// Null en cualquiera de los dos = queda vacío.</summary>
    public class ActualizarLoteDetalleCommand : IRequest<BaseResponse<DetalleCompraDto>>
    {
        public int IdDetalleCompra { get; set; }
        public string? Lote { get; set; }
        public DateTime? FechaVencimiento { get; set; }
    }
}
