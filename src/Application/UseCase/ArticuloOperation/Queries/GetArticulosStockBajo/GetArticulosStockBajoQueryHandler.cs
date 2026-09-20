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
                    .Where(a => a.Estado == "AC")
                    .ToList();

                var resultado = new List<ArticuloStockDto>();

                foreach (var articulo in articulos)
                {
                    var stock = await _stockService.GetStockActualAsync(articulo.Id);

                    // Si no cargaron Stock mínimo (es opcional, ej. en artículos con código
                    // compartido si no lo completaron), no hay forma de saber qué es "poco" para
                    // ese artículo — pero igual avisamos si se quedó directamente en 0 o menos,
                    // que es un caso que no necesita ninguna configuración para ser un problema.
                    var bajoStock = articulo.StockMinimo.HasValue
                        ? stock <= articulo.StockMinimo.Value
                        : stock <= 0;

                    if (bajoStock)
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
