using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.ArticuloOperation.Queries.GetStockArticulo
{
    public class GetStockArticuloQueryHandler : IRequestHandler<GetStockArticuloQuery, BaseResponse<ArticuloStockDto>>
    {
        private readonly IArticuloRepository _articuloRepository;
        private readonly IStockService _stockService;
        private readonly ILogger<GetStockArticuloQueryHandler> _logger;

        public GetStockArticuloQueryHandler(IArticuloRepository articuloRepository, IStockService stockService, ILogger<GetStockArticuloQueryHandler> logger)
        {
            _articuloRepository = articuloRepository;
            _stockService = stockService;
            _logger = logger;
        }

        public async Task<BaseResponse<ArticuloStockDto>> Handle(GetStockArticuloQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var articulo = await _articuloRepository.GetByIdAsync(request.IdArticulo);
                if (articulo == null)
                    return BaseResponse<ArticuloStockDto>.FailureResponse("Artículo no encontrado.");

                var stock = await _stockService.GetStockActualAsync(request.IdArticulo);

                var dto = new ArticuloStockDto
                {
                    IdArticulo = articulo.Id,
                    Codigo = articulo.Codigo,
                    Descripcion = articulo.Descripcion,
                    StockActual = stock,
                    StockMinimo = articulo.StockMinimo,
                    StockIdeal = articulo.StockIdeal
                };

                return BaseResponse<ArticuloStockDto>.SuccessResponse(dto, "Stock consultado correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar stock del artículo");
                return BaseResponse<ArticuloStockDto>.FailureResponse("Ocurrió un error al consultar el stock.");
            }
        }
    }
}
