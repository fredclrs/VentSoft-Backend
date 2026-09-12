using Application;
using Application.UseCase.MovimientoCajaOperation.Command.DeleteMovimientoCaja;
using Application.UseCase.MovimientoCajaOperation.Command.RegistrarMovimientoCaja;
using Application.UseCase.MovimientoCajaOperation.Queries.GetMovimientosDelDia;
using Domain.Common;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebVentSoft.API.Controllers
{
    /// <summary>Entradas/salidas de efectivo de la caja que no vienen de una Venta ni de una
    /// Devolución/Cambio (sacar plata para comprar algo puntual, un gasto suelto, etc.) — para
    /// que "Ventas del día" pueda calcular bien el efectivo esperado en caja. Mismo permiso que
    /// ese reporte: no hace falta poder vender para poder cerrar la caja del día.</summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = Permisos.VentasDelDia)]
    public class MovimientoCajaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MovimientoCajaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<MovimientoCajaDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<MovimientoCajaDto>), 400)]
        public async Task<IActionResult> Registrar([FromBody] RegistrarMovimientoCajaDto movimientoDto)
        {
            var response = await _mediator.Send(new RegistrarMovimientoCajaCommand { MovimientoDto = movimientoDto });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Todos los movimientos de caja de una fecha puntual.</summary>
        [HttpGet("delDia")]
        [ProducesResponseType(typeof(BaseResponse<List<MovimientoCajaDto>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<List<MovimientoCajaDto>>), 400)]
        public async Task<IActionResult> GetDelDia([FromQuery] DateTime fecha)
        {
            var response = await _mediator.Send(new GetMovimientosDelDiaQuery { Fecha = fecha });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Da de baja (lógica) un movimiento cargado por error.</summary>
        [HttpDelete]
        [ProducesResponseType(typeof(BaseResponse<MovimientoCajaDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<MovimientoCajaDto>), 400)]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var response = await _mediator.Send(new DeleteMovimientoCajaCommand { Id = id });
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
