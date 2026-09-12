using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.ArticuloOperation.Queries.GetArticulosStockBajo
{
    public class GetArticulosStockBajoQueryHandler : IRequestHandler<GetArticulosStockBajoQuery, BaseResponse<List<ArticuloStockDto>>>
    {
        private readonly IArticuloRepository _articuloRepository;
        private readonly IStockService _stockService;
        private readonly ILogger<GetArticulosStockBajoQueryHandler> _logger;

        public GetArticulosStockBajoQueryHandler(IArticuloRepository articuloRepository, IStockService stockService, ILogger<GetArticulosStockBajoQueryHandler> logger)
        {
            _articuloRepository = articuloRepository;
            _stockService = stockService;
            _logger = logger;
        }

        public async Task<BaseResponse<List<ArticuloStockDto>>> Handle(GetArticulosStockBajoQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var articulos = (await _articuloRepository.GetAllAsync())
                    .Where(a => a.Estado == "AC" && a.StockMinimo.HasValue)
                    .ToList();

                var resultado = new List<ArticuloStockDto>();

                foreach (var articulo in articulos)
                {
                    var stock = await _stockService.GetStockActualAsync(articulo.Id);
                    if (stock <= articulo.StockMinimo!.Value)
                    {
                        resultado.Add(new ArticuloStockDto
                        {
                            IdArticulo = articulo.Id,
                            Codigo = articulo.Codigo,
                            Descripcion = articulo.Descripcion,
                            StockActual = stock,
                            StockMinimo = articulo.StockMinimo,
                            StockIdeal = articulo.StockIdeal
                        });
                    }
                }

                return BaseResponse<List<ArticuloStockDto>>.SuccessResponse(resultado, "Consulta de stock bajo realizada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar artículos con stock bajo");
                return BaseResponse<List<ArticuloStockDto>>.FailureResponse("Ocurrió un error al consultar el stock bajo.");
            }
        }
    }
}
