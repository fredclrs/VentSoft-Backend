using Application;
using Application.UseCase.TemporadaOperation.Command.AddTemporada;
using Application.UseCase.TemporadaOperation.Command.DeleteTemporada;
using Application.UseCase.TemporadaOperation.Command.UpdateTemporada;
using Application.UseCase.TemporadaOperation.Queries.GetTemporadaById;
using Application.UseCase.TemporadaOperation.Queries.SearchTemporadas;
using Domain.Common;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebVentSoft.API.Controllers
{
    /// <summary>
    /// Configuración opcional de temporadas/campañas de venta (ej: "Verano", "Invierno",
    /// "Campaña siembra"), por rango de meses. Si un negocio no crea ninguna, el resto del
    /// sistema no se ve afectado en nada: solo el reporte "Ventas por temporada" la usa.
    /// Crear/editar/dar de baja es solo de Administrador; la búsqueda queda abierta a
    /// cualquier usuario autenticado porque los reportes de Vendedor (ej: Productos más
    /// vendidos) también necesitan leer la lista para el filtro de temporada.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TemporadaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TemporadaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>Crea una nueva temporada/campaña.</summary>
        [HttpPost]
        [Authorize(Roles = Permisos.Configuracion)]
        [ProducesResponseType(typeof(BaseResponse<TemporadaDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<TemporadaDto>), 400)]
        public async Task<IActionResult> Insert([FromBody] TemporadaDto dto)
        {
            var response = await _mediator.Send(new AddTemporadaCommand { TemporadaDto = dto });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Actualiza una temporada existente.</summary>
        [HttpPut]
        [Authorize(Roles = Permisos.Configuracion)]
        [ProducesResponseType(typeof(BaseResponse<TemporadaDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<TemporadaDto>), 400)]
        public async Task<IActionResult> Update([FromQuery] int id, [FromBody] TemporadaDto dto)
        {
            var response = await _mediator.Send(new UpdateTemporadaCommand { Id = id, TemporadaDto = dto });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Da de baja (lógica).</summary>
        [HttpDelete]
        [Authorize(Roles = Permisos.Configuracion)]
        [ProducesResponseType(typeof(BaseResponse<TemporadaDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<TemporadaDto>), 400)]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var response = await _mediator.Send(new DeleteTemporadaCommand { Id = id });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Devuelve por Id.</summary>
        [HttpGet("byId")]
        [ProducesResponseType(typeof(BaseResponse<TemporadaDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<TemporadaDto>), 400)]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var response = await _mediator.Send(new GetTemporadaByIdQuery { Id = id });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Busca por Nombre (o lista todas si no se indica).</summary>
        [HttpGet("search")]
        [ProducesResponseType(typeof(BaseResponse<List<TemporadaDto>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<List<TemporadaDto>>), 400)]
        public async Task<IActionResult> Search([FromQuery] string? nombre = null)
        {
            var response = await _mediator.Send(new SearchTemporadaQuery { Nombre = nombre });
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
