using Domain.Dtos;
using MediatR;

namespace Application.UseCase.CompraOperation.Queries.GetLotesPorVencer
{
    /// <summary>Renglones de compra (lotes) con Lote y/o vencimiento cargados. Sin DiasAnticipacion
    /// (null), trae todos — sirve para ver el historial completo de lotes de un artículo puntual
    /// (ver IdArticulo) sin importar cuán lejos esté su vencimiento.</summary>
    public class GetLotesPorVencerQuery : IRequest<BaseResponse<List<LoteVencimientoDto>>>
    {
        public int? DiasAnticipacion { get; set; } = 30;
        public int? IdArticulo { get; set; }
    }
}
