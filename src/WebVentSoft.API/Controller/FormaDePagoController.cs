using Application;
using Application.UseCase.FormaDePagoOperation.Command.AddFormaDePago;
using Application.UseCase.FormaDePagoOperation.Command.DeleteFormaDePago;
using Application.UseCase.FormaDePagoOperation.Command.UpdateFormaDePago;
using Application.UseCase.FormaDePagoOperation.Queries.GetFormaDePagoById;
using Application.UseCase.FormaDePagoOperation.Queries.SearchFormaDePagos;
using Domain.Common;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebVentSoft.API.Controllers
{
    /// <summary>Catálogo de configuración del negocio: solo Administrador.</summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = Permisos.Configuracion)]
    public class FormaDePagoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FormaDePagoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>Crea una nueva forma de pago.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<FormaDePagoDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<FormaDePagoDto>), 400)]
        public async Task<IActionResult> Insert([FromBody] FormaDePagoDto dto)
        {
            var response = await _mediator.Send(new AddFormaDePagoCommand { FormaDePagoDto = dto });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Actualiza forma de pago existente.</summary>
        [HttpPut]
        [ProducesResponseType(typeof(BaseResponse<FormaDePagoDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<FormaDePagoDto>), 400)]
        public async Task<IActionResult> Update([FromQuery] int id, [FromBody] FormaDePagoDto dto)
        {
            var response = await _mediator.Send(new UpdateFormaDePagoCommand { Id = id, FormaDePagoDto = dto });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Da de baja (lógica).</summary>
        [HttpDelete]
        [ProducesResponseType(typeof(BaseResponse<FormaDePagoDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<FormaDePagoDto>), 400)]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var response = await _mediator.Send(new DeleteFormaDePagoCommand { Id = id });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Devuelve por Id.</summary>
        [HttpGet("byId")]
        [ProducesResponseType(typeof(BaseResponse<FormaDePagoDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<FormaDePagoDto>), 400)]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var response = await _mediator.Send(new GetFormaDePagoByIdQuery { Id = id });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Busca por Nombre.</summary>
        [HttpGet("search")]
        [ProducesResponseType(typeof(BaseResponse<List<FormaDePagoDto>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<List<FormaDePagoDto>>), 400)]
        public async Task<IActionResult> Search([FromQuery] string? nombre = null)
        {
            var response = await _mediator.Send(new SearchFormaDePagoQuery { Nombre = nombre });
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
