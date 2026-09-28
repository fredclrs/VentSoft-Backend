using Application.Common;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.IaOperation.Command.LeerFactura
{
    /// <summary>
    /// Lee una foto de factura de compra con IA y devuelve una lista de renglones ya comparados
    /// contra el catálogo (marcando cuáles ya existen y cuáles no) — no crea ni modifica nada por
    /// su cuenta, es solo una sugerencia para que la persona revise y confirme en la pantalla de
    /// Compras.
    /// </summary>
    public class LeerFacturaCommandHandler : IRequestHandler<LeerFacturaCommand, BaseResponse<List<LineaFacturaDto>>>
    {
        private static string ConstruirPrompt(List<Caracteristica> catalogoCaracteristicas) =>
            "Esta imagen es una factura de compra de un negocio minorista. Identificá cada " +
            "producto/línea de la factura y devolveme ÚNICAMENTE un array JSON (sin texto antes " +
            "ni después, sin bloque de código markdown), donde cada elemento tenga exactamente " +
            "estas claves: \"descripcion\" (string, el nombre del producto SIN sus atributos de " +
            "variante, tal como lo llamaría este negocio en particular — ej. \"remera polo\" en " +
            "indumentaria, \"tornillo autorroscante\" en ferretería, \"ibuprofeno 400mg\" en " +
            "farmacia — si la factura no los separa del nombre, dejá la descripción completa acá " +
            "y \"caracteristicas\" vacío), \"caracteristicas\" (array — " +
            "ver abajo), \"cantidad\" (número entero), \"costoUnitario\" (número, el costo de UNA " +
            "unidad — si la factura solo trae el subtotal de la línea, dividí por la cantidad). " +
            IaHelpers.DescribirCaracteristicasParaPrompt(catalogoCaracteristicas) + " Si no podés " +
            "leer algún dato con confianza, hacé tu mejor estimación en vez de inventar un valor " +
            "absurdo. No incluyas totales, impuestos, ni líneas que no sean productos.";

        private readonly IRepository<ConfiguracionEmpresa> _configuracionRepository;
        private readonly IRepository<Caracteristica> _caracteristicaRepository;
        private readonly IArticuloRepository _articuloRepository;
        private readonly ICifradoService _cifradoService;
        private readonly IAnthropicClient _anthropicClient;
        private readonly ILogger<LeerFacturaCommandHandler> _logger;

        public LeerFacturaCommandHandler(
            IRepository<ConfiguracionEmpresa> configuracionRepository,
            IRepository<Caracteristica> caracteristicaRepository,
            IArticuloRepository articuloRepository,
            ICifradoService cifradoService,
            IAnthropicClient anthropicClient,
            ILogger<LeerFacturaCommandHandler> logger)
        {
            _configuracionRepository = configuracionRepository;
            _caracteristicaRepository = caracteristicaRepository;
            _articuloRepository = articuloRepository;
            _cifradoService = cifradoService;
            _anthropicClient = anthropicClient;
            _logger = logger;
        }

        public async Task<BaseResponse<List<LineaFacturaDto>>> Handle(LeerFacturaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var (apiKey, permiteCodigoCompartido, error) = await IaHelpers.ObtenerApiKeyAsync(_configuracionRepository, _cifradoService);
                if (apiKey == null)
                    return BaseResponse<List<LineaFacturaDto>>.FailureResponse(error!);

                var catalogoCaracteristicas = (await _caracteristicaRepository.GetAllAsync()).ToList();

                var respuestaTexto = await _anthropicClient.EnviarAsync(
                    apiKey,
                    ConstruirPrompt(catalogoCaracteristicas),
                    new ImagenAdjunta(request.ImagenBytes, request.MediaType),
                    cancellationToken);

                var lineas = JsonExtractor.ExtraerArray<LineaFacturaDto>(respuestaTexto);

                var articulos = (await _articuloRepository.GetAllWithCaracteristicasAsync())
                    .Where(a => a.Estado == "AC")
                    .ToList();

                foreach (var linea in lineas)
                {
                    IaHelpers.ResolverCaracteristicas(linea.Caracteristicas, catalogoCaracteristicas);
                    IaHelpers.Clasificar(linea, articulos, permiteCodigoCompartido);
                }

                return BaseResponse<List<LineaFacturaDto>>.SuccessResponse(lineas, "Factura leída correctamente.");
            }
            catch (AnthropicApiException ex)
            {
                return BaseResponse<List<LineaFacturaDto>>.FailureResponse(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al leer la factura con IA");
                return BaseResponse<List<LineaFacturaDto>>.FailureResponse(
                    "No se pudo leer la factura. Probá con otra foto (buena luz, bien enfocada) o cargala a mano.");
            }
        }
    }
}
