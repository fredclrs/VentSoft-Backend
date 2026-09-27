using Application.Common;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.IaOperation.Command.InterpretarListaProductos
{
    /// <summary>
    /// Interpreta una lista de texto libre escrita a mano (ej. "remera polo azul 4\nremera nike
    /// rojo talla M 5") y devuelve una lista de renglones ya comparados contra el catálogo — para
    /// cuando llega mercadería sin una factura formal. Igual que LeerFactura: solo sugiere, la
    /// persona confirma en Artículos.
    /// </summary>
    public class InterpretarListaProductosCommandHandler : IRequestHandler<InterpretarListaProductosCommand, BaseResponse<List<LineaTextoProductoDto>>>
    {
        private const string Prompt =
            "El siguiente texto es una lista escrita a mano por el dueño de un negocio de " +
            "indumentaria, describiendo productos que recibió (una prenda por línea, más o menos, " +
            "aunque puede venir todo junto). Para cada producto que identifiques, devolveme " +
            "ÚNICAMENTE un array JSON (sin texto antes ni después, sin bloque de código markdown) " +
            "donde cada elemento tenga exactamente estas claves: \"descripcion\" (string, el " +
            "nombre del producto sin la talla/color/cantidad, ej. \"remera polo\"), \"talla\" " +
            "(string o null si no se menciona), \"color\" (string o null si no se menciona), " +
            "\"cantidad\" (número entero — si no se menciona ninguna cantidad para un producto, " +
            "asumí 1). Texto a interpretar:\n\n";

        private readonly IRepository<ConfiguracionEmpresa> _configuracionRepository;
        private readonly IArticuloRepository _articuloRepository;
        private readonly ICifradoService _cifradoService;
        private readonly IAnthropicClient _anthropicClient;
        private readonly ILogger<InterpretarListaProductosCommandHandler> _logger;

        public InterpretarListaProductosCommandHandler(
            IRepository<ConfiguracionEmpresa> configuracionRepository,
            IArticuloRepository articuloRepository,
            ICifradoService cifradoService,
            IAnthropicClient anthropicClient,
            ILogger<InterpretarListaProductosCommandHandler> logger)
        {
            _configuracionRepository = configuracionRepository;
            _articuloRepository = articuloRepository;
            _cifradoService = cifradoService;
            _anthropicClient = anthropicClient;
            _logger = logger;
        }

        public async Task<BaseResponse<List<LineaTextoProductoDto>>> Handle(InterpretarListaProductosCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Texto))
                    return BaseResponse<List<LineaTextoProductoDto>>.FailureResponse("Escribí al menos un producto.");

                var (apiKey, permiteCodigoCompartido, error) = await IaHelpers.ObtenerApiKeyAsync(_configuracionRepository, _cifradoService);
                if (apiKey == null)
                    return BaseResponse<List<LineaTextoProductoDto>>.FailureResponse(error!);

                var respuestaTexto = await _anthropicClient.EnviarAsync(
                    apiKey,
                    Prompt + request.Texto,
                    imagen: null,
                    cancellationToken);

                var lineas = JsonExtractor.ExtraerArray<LineaTextoProductoDto>(respuestaTexto);

                var articulos = (await _articuloRepository.GetAllWithCaracteristicasAsync())
                    .Where(a => a.Estado == "AC")
                    .ToList();

                foreach (var linea in lineas)
                {
                    IaHelpers.Clasificar(linea, articulos, permiteCodigoCompartido);
                }

                return BaseResponse<List<LineaTextoProductoDto>>.SuccessResponse(lineas, "Lista interpretada correctamente.");
            }
            catch (AnthropicApiException ex)
            {
                return BaseResponse<List<LineaTextoProductoDto>>.FailureResponse(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al interpretar la lista de productos con IA");
                return BaseResponse<List<LineaTextoProductoDto>>.FailureResponse(
                    "No se pudo interpretar la lista. Revisá el texto o cargá los productos a mano.");
            }
        }
    }
}
