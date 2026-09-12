using Application;
using Application.UseCase.PromocionOperation.Command.AddPromocion;
using Application.UseCase.PromocionOperation.Command.DeletePromocion;
using Application.UseCase.PromocionOperation.Command.UpdatePromocion;
using Application.UseCase.PromocionOperation.Queries.GetPromocionById;
using Application.UseCase.PromocionOperation.Queries.SearchPromocions;
using Domain.Common;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebVentSoft.API.Controllers
{
    /// <summary>Catálogo de inventario: requiere el permiso "inventario".</summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = Permisos.Inventario)]
    public class PromocionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PromocionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>Crea una nueva promoción.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<PromocionDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<PromocionDto>), 400)]
        public async Task<IActionResult> Insert([FromBody] PromocionDto dto)
        {
            var response = await _mediator.Send(new AddPromocionCommand { PromocionDto = dto });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Actualiza promoción existente.</summary>
        [HttpPut]
        [ProducesResponseType(typeof(BaseResponse<PromocionDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<PromocionDto>), 400)]
        public async Task<IActionResult> Update([FromQuery] int id, [FromBody] PromocionDto dto)
        {
            var response = await _mediator.Send(new UpdatePromocionCommand { Id = id, PromocionDto = dto });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Da de baja (lógica).</summary>
        [HttpDelete]
        [ProducesResponseType(typeof(BaseResponse<PromocionDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<PromocionDto>), 400)]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var response = await _mediator.Send(new DeletePromocionCommand { Id = id });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Devuelve por Id.</summary>
        [HttpGet("byId")]
        [ProducesResponseType(typeof(BaseResponse<PromocionDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<PromocionDto>), 400)]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var response = await _mediator.Send(new GetPromocionByIdQuery { Id = id });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Busca por NombrePromocion.</summary>
        [HttpGet("search")]
        [ProducesResponseType(typeof(BaseResponse<List<PromocionDto>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<List<PromocionDto>>), 400)]
        public async Task<IActionResult> Search([FromQuery] string? nombrePromocion = null)
        {
            var response = await _mediator.Send(new SearchPromocionQuery { NombrePromocion = nombrePromocion });
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
