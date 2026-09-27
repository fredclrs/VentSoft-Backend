using Application;
using Application.UseCase.IaOperation.Command.InterpretarListaProductos;
using Application.UseCase.IaOperation.Command.LeerDocumentoCliente;
using Application.UseCase.IaOperation.Command.LeerFactura;
using Domain.Common;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebVentSoft.API.Controllers
{
    /// <summary>
    /// Funciones de IA (Anthropic Claude) — leer una factura o un documento por foto, interpretar
    /// una lista de productos escrita a mano. Ninguna de las tres crea ni modifica nada por su
    /// cuenta: solo leen/interpretan y devuelven una sugerencia para que la persona revise y
    /// confirme con los endpoints de siempre (Articulo, Compra, Cliente, AjusteStock). Requieren
    /// que el negocio haya configurado su propia API key en Configuración (ver
    /// ConfiguracionEmpresa.ClaveApiIACifrada) — si no, devuelven un error claro pidiendo eso.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class IaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public IaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>Lee una foto de factura de compra y devuelve sus renglones, marcando cuáles
        /// ya existen en el catálogo y cuáles no — para precargar una Compra.</summary>
        [HttpPost("leerFactura")]
        [Authorize(Roles = Permisos.Compras)]
        [ProducesResponseType(typeof(BaseResponse<List<LineaFacturaDto>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<List<LineaFacturaDto>>), 400)]
        public async Task<IActionResult> LeerFactura([FromBody] ImagenRequestDto imagenDto)
        {
            var response = await _mediator.Send(new LeerFacturaCommand
            {
                ImagenBytes = Convert.FromBase64String(imagenDto.ImagenBase64),
                MediaType = imagenDto.MediaType,
            });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        public class InterpretarListaProductosRequest
        {
            public string Texto { get; set; } = null!;
        }

        /// <summary>Interpreta una lista de productos escrita a mano y devuelve sus renglones,
        /// marcando cuáles ya existen en el catálogo y cuáles no — para dar de alta artículos sin
        /// una factura formal.</summary>
        [HttpPost("interpretarListaProductos")]
        [Authorize(Roles = Permisos.Inventario)]
        [ProducesResponseType(typeof(BaseResponse<List<LineaTextoProductoDto>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<List<LineaTextoProductoDto>>), 400)]
        public async Task<IActionResult> InterpretarListaProductos([FromBody] InterpretarListaProductosRequest request)
        {
            var response = await _mediator.Send(new InterpretarListaProductosCommand { Texto = request.Texto });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Lee una foto de un documento de identidad y devuelve nombre + número de
        /// documento — para precargar el alta de un Cliente.</summary>
        [HttpPost("leerDocumentoCliente")]
        [Authorize(Roles = Permisos.Ventas)]
        [ProducesResponseType(typeof(BaseResponse<DatosClienteExtraidosDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<DatosClienteExtraidosDto>), 400)]
        public async Task<IActionResult> LeerDocumentoCliente([FromBody] ImagenRequestDto imagenDto)
        {
            var response = await _mediator.Send(new LeerDocumentoClienteCommand
            {
                ImagenBytes = Convert.FromBase64String(imagenDto.ImagenBase64),
                MediaType = imagenDto.MediaType,
            });
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
