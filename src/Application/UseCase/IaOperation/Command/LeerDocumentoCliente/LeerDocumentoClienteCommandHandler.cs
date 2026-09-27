using Application.Common;
using Application.Interfaces;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.IaOperation.Command.LeerDocumentoCliente
{
    /// <summary>
    /// Lee una foto de un documento de identidad (DNI/cédula) con IA y devuelve nombre y número
    /// de documento — para precargar el alta de Cliente. No crea ningún Cliente por su cuenta, la
    /// persona revisa/corrige y guarda con el flujo de siempre.
    /// </summary>
    public class LeerDocumentoClienteCommandHandler : IRequestHandler<LeerDocumentoClienteCommand, BaseResponse<DatosClienteExtraidosDto>>
    {
        private const string Prompt =
            "Esta imagen es un documento de identidad (DNI, cédula, u otro documento oficial). " +
            "Devolveme ÚNICAMENTE un objeto JSON (sin texto antes ni después, sin bloque de código " +
            "markdown) con exactamente estas claves: \"nombre\" (el nombre completo de la persona " +
            "tal como figura en el documento, o null si no se puede leer), \"documentoIdentidad\" " +
            "(el número de documento, o null si no se puede leer).";

        private readonly IRepository<ConfiguracionEmpresa> _configuracionRepository;
        private readonly ICifradoService _cifradoService;
        private readonly IAnthropicClient _anthropicClient;
        private readonly ILogger<LeerDocumentoClienteCommandHandler> _logger;

        public LeerDocumentoClienteCommandHandler(
            IRepository<ConfiguracionEmpresa> configuracionRepository,
            ICifradoService cifradoService,
            IAnthropicClient anthropicClient,
            ILogger<LeerDocumentoClienteCommandHandler> logger)
        {
            _configuracionRepository = configuracionRepository;
            _cifradoService = cifradoService;
            _anthropicClient = anthropicClient;
            _logger = logger;
        }

        public async Task<BaseResponse<DatosClienteExtraidosDto>> Handle(LeerDocumentoClienteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var (apiKey, _, error) = await IaHelpers.ObtenerApiKeyAsync(_configuracionRepository, _cifradoService);
                if (apiKey == null)
                    return BaseResponse<DatosClienteExtraidosDto>.FailureResponse(error!);

                var respuestaTexto = await _anthropicClient.EnviarAsync(
                    apiKey,
                    Prompt,
                    new ImagenAdjunta(request.ImagenBytes, request.MediaType),
                    cancellationToken);

                var datos = JsonExtractor.ExtraerObjeto<DatosClienteExtraidosDto>(respuestaTexto);

                return BaseResponse<DatosClienteExtraidosDto>.SuccessResponse(datos, "Documento leído correctamente.");
            }
            catch (AnthropicApiException ex)
            {
                return BaseResponse<DatosClienteExtraidosDto>.FailureResponse(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al leer el documento de cliente con IA");
                return BaseResponse<DatosClienteExtraidosDto>.FailureResponse(
                    "No se pudo leer el documento. Probá con otra foto (buena luz, bien enfocada) o cargalo a mano.");
            }
        }
    }
}
