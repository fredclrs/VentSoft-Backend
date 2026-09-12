using Application;
using Application.UseCase.PagoOperation.Command.RegistrarPago;
using Application.UseCase.PagoOperation.Queries.GetDeudaProveedor;
using Application.UseCase.PagoOperation.Queries.GetPagosByProveedor;
using Domain.Common;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebVentSoft.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PagoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>Registra un pago a un proveedor; se aplica automáticamente contra sus compras con saldo pendiente (más antiguas primero).</summary>
        [HttpPost]
        [Authorize(Roles = Permisos.Pagos)]
        [ProducesResponseType(typeof(BaseResponse<PagoDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<PagoDto>), 400)]
        public async Task<IActionResult> RegistrarPago([FromBody] RegistrarPagoDto pagoDto)
        {
            var response = await _mediator.Send(new RegistrarPagoCommand { PagoDto = pagoDto });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Deuda actual (saldo pendiente total) con un proveedor.</summary>
        [HttpGet("deuda")]
        [Authorize(Roles = Permisos.CuentasPorPagar)]
        [ProducesResponseType(typeof(BaseResponse<ProveedorDeudaDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<ProveedorDeudaDto>), 400)]
        public async Task<IActionResult> GetDeuda([FromQuery] int idProveedor)
        {
            var response = await _mediator.Send(new GetDeudaProveedorQuery { IdProveedor = idProveedor });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Historial de pagos a un proveedor (más recientes primero).</summary>
        [HttpGet("byProveedor")]
        [Authorize(Roles = Permisos.Pagos)]
        [ProducesResponseType(typeof(BaseResponse<List<PagoDto>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<List<PagoDto>>), 400)]
        public async Task<IActionResult> GetByProveedor([FromQuery] int idProveedor)
        {
            var response = await _mediator.Send(new GetPagosByProveedorQuery { IdProveedor = idProveedor });
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
