using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.ArticuloOperation.Queries.GetStockTodos
{
    public class GetStockTodosQueryHandler : IRequestHandler<GetStockTodosQuery, BaseResponse<List<ArticuloStockDto>>>
    {
        private readonly IArticuloRepository _articuloRepository;
        private readonly IStockService _stockService;
        private readonly ILogger<GetStockTodosQueryHandler> _logger;

        public GetStockTodosQueryHandler(IArticuloRepository articuloRepository, IStockService stockService, ILogger<GetStockTodosQueryHandler> logger)
        {
            _articuloRepository = articuloRepository;
            _stockService = stockService;
            _logger = logger;
        }

        public async Task<BaseResponse<List<ArticuloStockDto>>> Handle(GetStockTodosQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var articulos = (await _articuloRepository.GetAllAsync())
                    .Where(a => a.Estado == "AC")
                    .ToList();

                var stockPorArticulo = await _stockService.GetStockPorArticuloAsync();

                var resultado = articulos
                    .Select(a => new ArticuloStockDto
                    {
                        IdArticulo = a.Id,
                        Codigo = a.Codigo,
                        Descripcion = a.Descripcion,
                        StockActual = stockPorArticulo.GetValueOrDefault(a.Id),
                        StockMinimo = a.StockMinimo,
                        StockIdeal = a.StockIdeal
                    })
                    .ToList();

                return BaseResponse<List<ArticuloStockDto>>.SuccessResponse(resultado, "Stock consultado correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar el stock de todos los artículos");
                return BaseResponse<List<ArticuloStockDto>>.FailureResponse("Ocurrió un error al consultar el stock.");
            }
        }
    }
}
