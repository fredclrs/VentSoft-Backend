using Application;
using Application.UseCase.EntregaBienOperation.Command.RegistrarEntregaBien;
using Application.UseCase.EntregaBienOperation.Queries.GetEntregasByCliente;
using Application.UseCase.EntregaBienOperation.Queries.GetEntregasPendientesByCliente;
using Domain.Common;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebVentSoft.API.Controllers
{
    /// <summary>Boletas de entrega de bienes (pago en especie de una deuda). Registrar una boleta
    /// es operativo (permiso Entregas); consultarlas queda abierto porque también las necesita
    /// quien liquida (permiso Liquidaciones, ver LiquidacionController).</summary>
    [ApiController]
    [Route("api/[controller]")]
    public class EntregaBienController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EntregaBienController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>Registra una boleta de entrega (el precio es opcional acá).</summary>
        [HttpPost]
        [Authorize(Roles = Permisos.Entregas)]
        [ProducesResponseType(typeof(BaseResponse<EntregaBienDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<EntregaBienDto>), 400)]
        public async Task<IActionResult> RegistrarEntrega([FromBody] RegistrarEntregaBienDto entregaDto)
        {
            var response = await _mediator.Send(new RegistrarEntregaBienCommand { EntregaDto = entregaDto });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Boletas de un cliente que todavía no se incluyeron en ninguna liquidación.</summary>
        [HttpGet("pendientesByCliente")]
        [ProducesResponseType(typeof(BaseResponse<List<EntregaBienDto>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<List<EntregaBienDto>>), 400)]
        public async Task<IActionResult> GetPendientesByCliente([FromQuery] int idCliente)
        {
            var response = await _mediator.Send(new GetEntregasPendientesByClienteQuery { IdCliente = idCliente });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Historial completo (pendientes y liquidadas) de un cliente.</summary>
        [HttpGet("byCliente")]
        [ProducesResponseType(typeof(BaseResponse<List<EntregaBienDto>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<List<EntregaBienDto>>), 400)]
        public async Task<IActionResult> GetByCliente([FromQuery] int idCliente)
        {
            var response = await _mediator.Send(new GetEntregasByClienteQuery { IdCliente = idCliente });
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
