using Application;
using Application.UseCase.CompraOperation.Command.ActualizarLoteDetalle;
using Application.UseCase.CompraOperation.Command.AnularCompra;
using Application.UseCase.CompraOperation.Command.RegistrarCompra;
using Application.UseCase.CompraOperation.Queries.GetCompraById;
using Application.UseCase.CompraOperation.Queries.GetComprasByProveedor;
using Application.UseCase.CompraOperation.Queries.GetLotesPorVencer;
using Domain.Common;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebVentSoft.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompraController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompraController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>Registra una compra a un proveedor con todos sus renglones (artículos) en una sola operación,
        /// y actualiza el costo promedio ponderado de cada artículo comprado.</summary>
        [HttpPost]
        [Authorize(Roles = Permisos.Compras)]
        [ProducesResponseType(typeof(BaseResponse<CompraDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<CompraDto>), 400)]
        public async Task<IActionResult> RegistrarCompra([FromBody] RegistrarCompraDto compraDto)
        {
            var response = await _mediator.Send(new RegistrarCompraCommand { CompraDto = compraDto });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Carga o corrige el Lote/Fecha de vencimiento de un renglón de una compra ya
        /// registrada. No toca cantidades, costos, stock ni pagos.</summary>
        [HttpPut("detalle/lote")]
        [Authorize(Roles = Permisos.Compras)]
        [ProducesResponseType(typeof(BaseResponse<DetalleCompraDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<DetalleCompraDto>), 400)]
        public async Task<IActionResult> ActualizarLoteDetalle([FromBody] ActualizarLoteDetalleCommand command)
        {
            var response = await _mediator.Send(command);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Anula una compra registrada por error (queda guardada como "AN" con motivo,
        /// quién y cuándo). Permiso propio: no alcanza con poder comprar.</summary>
        [HttpPost("anular")]
        [Authorize(Roles = Permisos.Anular)]
        [ProducesResponseType(typeof(BaseResponse<CompraDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<CompraDto>), 400)]
        public async Task<IActionResult> Anular([FromBody] AnularCompraCommand command)
        {
            // Quién anula sale del token, no de lo que mande la pantalla.
            command.Usuario = User.Identity?.Name ?? User.FindFirst("unique_name")?.Value;
            var response = await _mediator.Send(command);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Devuelve una compra (con sus renglones) por Id.</summary>
        [HttpGet("byId")]
        [Authorize(Roles = Permisos.Compras)]
        [ProducesResponseType(typeof(BaseResponse<CompraDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<CompraDto>), 400)]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var response = await _mediator.Send(new GetCompraByIdQuery { Id = id });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Lista las compras registradas a un proveedor (más recientes primero).</summary>
        [HttpGet("byProveedor")]
        [Authorize(Roles = Permisos.Compras)]
        [ProducesResponseType(typeof(BaseResponse<List<CompraDto>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<List<CompraDto>>), 400)]
        public async Task<IActionResult> GetByProveedor([FromQuery] int idProveedor)
        {
            var response = await _mediator.Send(new GetComprasByProveedorQuery { IdProveedor = idProveedor });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Lotes (renglones de compra) con Lote y/o vencimiento cargados. Con
        /// diasAnticipacion (default 30) solo trae los que vencen dentro de esos días (o ya
        /// vencidos); mandando diasAnticipacion=null trae todos — para ver el historial completo
        /// de lotes de un artículo puntual (idArticulo), sin importar cuán lejos venza.
        /// Solo trae resultados para negocios que efectivamente cargan Lote/FechaVencimiento al comprar.</summary>
        [HttpGet("lotesPorVencer")]
        [ProducesResponseType(typeof(BaseResponse<List<LoteVencimientoDto>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<List<LoteVencimientoDto>>), 400)]
        public async Task<IActionResult> GetLotesPorVencer([FromQuery] int? diasAnticipacion = 30, [FromQuery] int? idArticulo = null)
        {
            var response = await _mediator.Send(new GetLotesPorVencerQuery { DiasAnticipacion = diasAnticipacion, IdArticulo = idArticulo });
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
