using Application.Interfaces.Repositories;
using Domain.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.CompraOperation.Queries.GetLotesPorVencer
{
    public class GetLotesPorVencerQueryHandler : IRequestHandler<GetLotesPorVencerQuery, BaseResponse<List<LoteVencimientoDto>>>
    {
        private readonly ICompraRepository _repository;
        private readonly ILogger<GetLotesPorVencerQueryHandler> _logger;

        public GetLotesPorVencerQueryHandler(ICompraRepository repository, ILogger<GetLotesPorVencerQueryHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<BaseResponse<List<LoteVencimientoDto>>> Handle(GetLotesPorVencerQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var detalles = await _repository.GetLotesPorVencerAsync(request.DiasAnticipacion, request.IdArticulo);

                var resultado = detalles.Select(d => new LoteVencimientoDto
                {
                    IdDetalleCompra = d.Id,
                    IdArticulo = d.IdArticulo,
                    CodigoArticulo = d.Articulo.Codigo,
                    DescripcionArticulo = d.Articulo.Descripcion,
                    Lote = d.Lote,
                    FechaVencimiento = d.FechaVencimiento,
                    Cantidad = d.Cantidad,
                    IdCompra = d.IdCompra,
                    FechaCompra = d.Compra.Fecha
                }).ToList();

                return BaseResponse<List<LoteVencimientoDto>>.SuccessResponse(resultado, "Consulta de vencimientos realizada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar lotes por vencer");
                return BaseResponse<List<LoteVencimientoDto>>.FailureResponse("Ocurrió un error al consultar los lotes por vencer.");
            }
        }
    }
}
