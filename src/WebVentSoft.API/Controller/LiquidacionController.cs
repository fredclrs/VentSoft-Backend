using Application;
using Application.UseCase.LiquidacionOperation.Command.RegistrarLiquidacion;
using Application.UseCase.LiquidacionOperation.Queries.GetLiquidacionesByCliente;
using Domain.Common;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebVentSoft.API.Controllers
{
    /// <summary>Liquidación de boletas de entrega (pago en especie) contra la deuda general de
    /// un cliente. Más sensible que registrar una boleta: fija precio y cierra cuentas.</summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = Permisos.Liquidaciones)]
    public class LiquidacionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LiquidacionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>Liquida una o más boletas pendientes de un cliente contra su deuda general.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<LiquidacionDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<LiquidacionDto>), 400)]
        public async Task<IActionResult> RegistrarLiquidacion([FromBody] RegistrarLiquidacionDto liquidacionDto)
        {
            var response = await _mediator.Send(new RegistrarLiquidacionCommand { LiquidacionDto = liquidacionDto });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Historial de liquidaciones de un cliente.</summary>
        [HttpGet("byCliente")]
        [ProducesResponseType(typeof(BaseResponse<List<LiquidacionDto>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<List<LiquidacionDto>>), 400)]
        public async Task<IActionResult> GetByCliente([FromQuery] int idCliente)
        {
            var response = await _mediator.Send(new GetLiquidacionesByClienteQuery { IdCliente = idCliente });
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
