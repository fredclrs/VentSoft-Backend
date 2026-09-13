using Application;
using Application.UseCase.AjusteStockOperation.Command.DeleteAjusteStock;
using Application.UseCase.AjusteStockOperation.Command.RegistrarAjusteStock;
using Application.UseCase.AjusteStockOperation.Queries.GetAjustesByArticulo;
using Domain.Common;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebVentSoft.API.Controllers
{
    /// <summary>Correcciones manuales de stock (rotura, vencimiento, robo, conteo real distinto)
    /// que no vienen de una Compra ni de una Venta. Mismo permiso que el resto del catálogo de
    /// inventario (Artículos, Familias, etc.).</summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = Permisos.Inventario)]
    public class AjusteStockController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AjusteStockController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>Registra un ajuste de stock. En una SALIDA, valida que haya stock suficiente
        /// antes de confirmar.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<AjusteStockDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<AjusteStockDto>), 400)]
        public async Task<IActionResult> Registrar([FromBody] RegistrarAjusteStockDto ajusteDto)
        {
            var response = await _mediator.Send(new RegistrarAjusteStockCommand { AjusteDto = ajusteDto });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Historial de ajustes de un artículo puntual (más recientes primero).</summary>
        [HttpGet("byArticulo")]
        [ProducesResponseType(typeof(BaseResponse<List<AjusteStockDto>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<List<AjusteStockDto>>), 400)]
        public async Task<IActionResult> GetByArticulo([FromQuery] int idArticulo)
        {
            var response = await _mediator.Send(new GetAjustesByArticuloQuery { IdArticulo = idArticulo });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Da de baja (lógica) un ajuste cargado por error.</summary>
        [HttpDelete]
        [ProducesResponseType(typeof(BaseResponse<AjusteStockDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<AjusteStockDto>), 400)]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var response = await _mediator.Send(new DeleteAjusteStockCommand { Id = id });
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
