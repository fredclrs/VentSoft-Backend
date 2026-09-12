using Application;
using Application.UseCase.CobroOperation.Command.RegistrarCobro;
using Application.UseCase.CobroOperation.Queries.GetCobrosByCliente;
using Application.UseCase.CobroOperation.Queries.GetDeudaCliente;
using Domain.Common;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebVentSoft.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CobroController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CobroController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>Registra un cobro a un cliente; se aplica automáticamente contra sus ventas con saldo pendiente (más antiguas primero).</summary>
        [HttpPost]
        [Authorize(Roles = Permisos.Cobros)]
        [ProducesResponseType(typeof(BaseResponse<CobroDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<CobroDto>), 400)]
        public async Task<IActionResult> RegistrarCobro([FromBody] RegistrarCobroDto cobroDto)
        {
            var response = await _mediator.Send(new RegistrarCobroCommand { CobroDto = cobroDto });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Deuda actual (saldo pendiente total) de un cliente.</summary>
        [HttpGet("deuda")]
        [Authorize(Roles = Permisos.CuentasPorCobrar)]
        [ProducesResponseType(typeof(BaseResponse<ClienteDeudaDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<ClienteDeudaDto>), 400)]
        public async Task<IActionResult> GetDeuda([FromQuery] int idCliente)
        {
            var response = await _mediator.Send(new GetDeudaClienteQuery { IdCliente = idCliente });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Historial de cobros de un cliente (más recientes primero).</summary>
        [HttpGet("byCliente")]
        [Authorize(Roles = Permisos.Cobros)]
        [ProducesResponseType(typeof(BaseResponse<List<CobroDto>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<List<CobroDto>>), 400)]
        public async Task<IActionResult> GetByCliente([FromQuery] int idCliente)
        {
            var response = await _mediator.Send(new GetCobrosByClienteQuery { IdCliente = idCliente });
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
