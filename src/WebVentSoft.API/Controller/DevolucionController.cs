using Application;
using Application.UseCase.DevolucionOperation.Command.RegistrarDevolucion;
using Application.UseCase.DevolucionOperation.Queries.GetDevolucionesByCliente;
using Application.UseCase.DevolucionOperation.Queries.GetDevolucionesByVenta;
using Application.UseCase.DevolucionOperation.Queries.GetDevolucionesDelDia;
using Domain.Common;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebVentSoft.API.Controllers
{
    /// <summary>Devoluciones y cambios de artículos de una Venta ya registrada.</summary>
    [ApiController]
    [Route("api/[controller]")]
    public class DevolucionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DevolucionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>Registra una devolución (o un cambio, si trae ArticulosCambio) de artículos de una
        /// venta existente. Usa el mismo permiso que Ventas: quien puede vender, puede procesar
        /// devoluciones/cambios.</summary>
        [HttpPost]
        [Authorize(Roles = Permisos.Ventas)]
        [ProducesResponseType(typeof(BaseResponse<DevolucionVentaDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<DevolucionVentaDto>), 400)]
        public async Task<IActionResult> RegistrarDevolucion([FromBody] RegistrarDevolucionDto devolucionDto)
        {
            var response = await _mediator.Send(new RegistrarDevolucionCommand { DevolucionDto = devolucionDto });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Devoluciones/cambios ya registrados contra una venta puntual.</summary>
        [HttpGet("byVenta")]
        [Authorize(Roles = Permisos.Ventas)]
        [ProducesResponseType(typeof(BaseResponse<List<DevolucionVentaDto>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<List<DevolucionVentaDto>>), 400)]
        public async Task<IActionResult> GetByVenta([FromQuery] int idVenta)
        {
            var response = await _mediator.Send(new GetDevolucionesByVentaQuery { IdVenta = idVenta });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Historial de devoluciones/cambios de un cliente (más recientes primero).</summary>
        [HttpGet("byCliente")]
        [Authorize(Roles = Permisos.Ventas)]
        [ProducesResponseType(typeof(BaseResponse<List<DevolucionVentaDto>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<List<DevolucionVentaDto>>), 400)]
        public async Task<IActionResult> GetByCliente([FromQuery] int idCliente)
        {
            var response = await _mediator.Send(new GetDevolucionesByClienteQuery { IdCliente = idCliente });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Todas las devoluciones/cambios (de cualquier cliente) de una fecha puntual — para
        /// saber cuánto efectivo entró/salió de la caja por eso ese día. Mismo permiso que "Ventas del
        /// día" (no el de "Ventas"): no hace falta poder vender para poder cerrar la caja del día.</summary>
        [HttpGet("delDia")]
        [Authorize(Roles = Permisos.VentasDelDia)]
        [ProducesResponseType(typeof(BaseResponse<List<DevolucionVentaDto>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<List<DevolucionVentaDto>>), 400)]
        public async Task<IActionResult> GetDelDia([FromQuery] DateTime fecha)
        {
            var response = await _mediator.Send(new GetDevolucionesDelDiaQuery { Fecha = fecha });
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
