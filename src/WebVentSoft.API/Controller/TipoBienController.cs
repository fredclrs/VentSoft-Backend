using Application;
using Application.UseCase.TipoBienOperation.Command.AddTipoBien;
using Application.UseCase.TipoBienOperation.Command.DeleteTipoBien;
using Application.UseCase.TipoBienOperation.Command.UpdateTipoBien;
using Application.UseCase.TipoBienOperation.Queries.GetTipoBienById;
using Application.UseCase.TipoBienOperation.Queries.SearchTipoBien;
using Domain.Common;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebVentSoft.API.Controllers
{
    /// <summary>Catálogo de tipos de bien aceptados como pago en especie (ej: "Soja", en
    /// toneladas). Dar de alta/editar/borrar es Configuración; consultarlo queda abierto a
    /// cualquier autenticado porque lo necesitan Entregas y Liquidaciones, que son permisos
    /// distintos de Configuración.</summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TipoBienController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TipoBienController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>Crea un nuevo tipo de bien.</summary>
        [HttpPost]
        [Authorize(Roles = Permisos.Configuracion)]
        [ProducesResponseType(typeof(BaseResponse<TipoBienDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<TipoBienDto>), 400)]
        public async Task<IActionResult> Insert([FromBody] TipoBienDto dto)
        {
            var response = await _mediator.Send(new AddTipoBienCommand { TipoBienDto = dto });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Actualiza un tipo de bien existente.</summary>
        [HttpPut]
        [Authorize(Roles = Permisos.Configuracion)]
        [ProducesResponseType(typeof(BaseResponse<TipoBienDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<TipoBienDto>), 400)]
        public async Task<IActionResult> Update([FromQuery] int id, [FromBody] TipoBienDto dto)
        {
            var response = await _mediator.Send(new UpdateTipoBienCommand { Id = id, TipoBienDto = dto });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Da de baja (lógica).</summary>
        [HttpDelete]
        [Authorize(Roles = Permisos.Configuracion)]
        [ProducesResponseType(typeof(BaseResponse<TipoBienDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<TipoBienDto>), 400)]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var response = await _mediator.Send(new DeleteTipoBienCommand { Id = id });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Devuelve por Id.</summary>
        [HttpGet("byId")]
        [ProducesResponseType(typeof(BaseResponse<TipoBienDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<TipoBienDto>), 400)]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var response = await _mediator.Send(new GetTipoBienByIdQuery { Id = id });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Busca por Nombre.</summary>
        [HttpGet("search")]
        [ProducesResponseType(typeof(BaseResponse<List<TipoBienDto>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<List<TipoBienDto>>), 400)]
        public async Task<IActionResult> Search([FromQuery] string? nombre = null)
        {
            var response = await _mediator.Send(new SearchTipoBienQuery { Nombre = nombre });
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
