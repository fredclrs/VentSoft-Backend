using Application;
using Application.UseCase.VentaOperation.Command.RegistrarVenta;
using Application.UseCase.VentaOperation.Queries.GetVentaById;
using Application.UseCase.VentaOperation.Queries.GetVentasByCliente;
using Application.UseCase.VentaOperation.Queries.GetVentasDelDia;
using Application.UseCase.VentaOperation.Queries.GetVentasTodas;
using Domain.Common;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebVentSoft.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VentaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>Registra una venta a un cliente con todos sus renglones (artículos) en una sola operación.
        /// Valida stock disponible por artículo antes de confirmar.</summary>
        [HttpPost]
        [Authorize(Roles = Permisos.Ventas)]
        [ProducesResponseType(typeof(BaseResponse<VentaDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<VentaDto>), 400)]
        public async Task<IActionResult> RegistrarVenta([FromBody] RegistrarVentaDto ventaDto)
        {
            var response = await _mediator.Send(new RegistrarVentaCommand { VentaDto = ventaDto });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Devuelve una venta (con sus renglones) por Id.</summary>
        [HttpGet("byId")]
        [Authorize(Roles = Permisos.Ventas)]
        [ProducesResponseType(typeof(BaseResponse<VentaDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<VentaDto>), 400)]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var response = await _mediator.Send(new GetVentaByIdQuery { Id = id });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Lista las ventas registradas a un cliente (más recientes primero).</summary>
        [HttpGet("byCliente")]
        [Authorize(Roles = Permisos.Ventas)]
        [ProducesResponseType(typeof(BaseResponse<List<VentaDto>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<List<VentaDto>>), 400)]
        public async Task<IActionResult> GetByCliente([FromQuery] int idCliente)
        {
            var response = await _mediator.Send(new GetVentasByClienteQuery { IdCliente = idCliente });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Todas las ventas (de cualquier cliente) de una fecha puntual — reporte "Ventas del día".
        /// Es un permiso aparte de "Ventas": no hace falta poder vender para poder cerrar la caja del día.</summary>
        [HttpGet("delDia")]
        [Authorize(Roles = Permisos.VentasDelDia)]
        [ProducesResponseType(typeof(BaseResponse<List<VentaDto>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<List<VentaDto>>), 400)]
        public async Task<IActionResult> GetDelDia([FromQuery] DateTime fecha)
        {
            var response = await _mediator.Send(new GetVentasDelDiaQuery { Fecha = fecha });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Todas las ventas activas, de cualquier cliente/vendedor — reportes "Ventas por
        /// vendedor" y "Ventas por artículo". Permiso aparte de "Ventas": deja ver el desempeño de
        /// todos los vendedores sin que haga falta poder vender.</summary>
        [HttpGet("todas")]
        [Authorize(Roles = Permisos.VentasPorVendedor)]
        [ProducesResponseType(typeof(BaseResponse<List<VentaDto>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<List<VentaDto>>), 400)]
        public async Task<IActionResult> GetTodas()
        {
            var response = await _mediator.Send(new GetVentasTodasQuery());
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
