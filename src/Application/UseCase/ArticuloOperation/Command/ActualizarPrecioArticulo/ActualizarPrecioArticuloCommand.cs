using Domain.Dtos;
using MediatR;

namespace Application.UseCase.ArticuloOperation.Command.ActualizarPrecioArticulo
{
    /// <summary>Aplica un precio sugerido (ver PrecioSugeridoDto) que el cajero confirmó después
    /// de una Compra. Toca únicamente Precio — nada más del artículo.</summary>
    public class ActualizarPrecioArticuloCommand : IRequest<BaseResponse<ArticuloDto>>
    {
        public int IdArticulo { get; set; }
        public double Precio { get; set; }
    }
}
