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
        private static string ConstruirPrompt(List<Caracteristica> catalogoCaracteristicas) =>
            "El siguiente texto es una lista escrita a mano por el dueño de un negocio, " +
            "describiendo productos que recibió. Devolveme ÚNICAMENTE un array " +
            "JSON (sin texto antes ni después, sin bloque de código markdown) con UN elemento por " +
            "cada variante, no uno por producto — cada elemento " +
            "con exactamente estas claves: \"descripcion\" (string, el nombre del producto SIN sus " +
            "atributos de variante ni la cantidad, ej. \"remera nike\"), \"caracteristicas\" " +
            "(array — ver abajo), \"cantidad\" (número entero — si no se menciona ninguna " +
            "cantidad, asumí 1). " +
            IaHelpers.DescribirCaracteristicasParaPrompt(catalogoCaracteristicas) +
            " Un caso MUY común: el nombre del producto se escribe una sola vez, seguido de " +
            "varias líneas o frases separadas por coma que son puros atributos/cantidades de ESE " +
            "MISMO producto (ej. \"Remera Nike talla L color azul stock 4, talla M color blanco " +
            "stock 3, talla XXL color azul stock 3\" son 3 variantes de \"Remera Nike\", no 3 " +
            "productos distintos ni productos sin nombre) — en ese caso repetí la MISMA " +
            "descripción en las 3, nunca la dejes en blanco ni inventes un nombre distinto. " +
            "\"stock\" en el texto significa lo mismo que \"cantidad\". Texto a interpretar:\n\n";

        private readonly IRepository<ConfiguracionEmpresa> _configuracionRepository;
        private readonly IRepository<Caracteristica> _caracteristicaRepository;
        private readonly IArticuloRepository _articuloRepository;
        private readonly ICifradoService _cifradoService;
        private readonly IAnthropicClient _anthropicClient;
        private readonly ILogger<InterpretarListaProductosCommandHandler> _logger;

        public InterpretarListaProductosCommandHandler(
            IRepository<ConfiguracionEmpresa> configuracionRepository,
            IRepository<Caracteristica> caracteristicaRepository,
            IArticuloRepository articuloRepository,
            ICifradoService cifradoService,
            IAnthropicClient anthropicClient,
            ILogger<InterpretarListaProductosCommandHandler> logger)
        {
            _configuracionRepository = configuracionRepository;
            _caracteristicaRepository = caracteristicaRepository;
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

                var catalogoCaracteristicas = (await _caracteristicaRepository.GetAllAsync()).ToList();

                var respuestaTexto = await _anthropicClient.EnviarAsync(
                    apiKey,
                    ConstruirPrompt(catalogoCaracteristicas) + request.Texto,
                    imagen: null,
                    cancellationToken);

                var lineas = JsonExtractor.ExtraerArray<LineaTextoProductoDto>(respuestaTexto);

                var articulos = (await _articuloRepository.GetAllWithCaracteristicasAsync())
                    .Where(a => a.Estado == "AC")
                    .ToList();

                foreach (var linea in lineas)
                {
                    IaHelpers.ResolverCaracteristicas(linea.Caracteristicas, catalogoCaracteristicas);
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
