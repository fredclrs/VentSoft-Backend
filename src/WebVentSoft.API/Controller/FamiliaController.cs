using Application;
using Application.UseCase.FamiliaOperation.Command.AddFamilia;
using Application.UseCase.FamiliaOperation.Command.DeleteFamilia;
using Application.UseCase.FamiliaOperation.Command.UpdateFamilia;
using Application.UseCase.FamiliaOperation.Queries.GetFamiliaById;
using Application.UseCase.FamiliaOperation.Queries.SearchFamilias;
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
    public class FamiliaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FamiliaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>Crea una nueva familia.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<FamiliaDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<FamiliaDto>), 400)]
        public async Task<IActionResult> Insert([FromBody] FamiliaDto dto)
        {
            var response = await _mediator.Send(new AddFamiliaCommand { FamiliaDto = dto });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Actualiza familia existente.</summary>
        [HttpPut]
        [ProducesResponseType(typeof(BaseResponse<FamiliaDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<FamiliaDto>), 400)]
        public async Task<IActionResult> Update([FromQuery] int id, [FromBody] FamiliaDto dto)
        {
            var response = await _mediator.Send(new UpdateFamiliaCommand { Id = id, FamiliaDto = dto });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Da de baja (lógica).</summary>
        [HttpDelete]
        [ProducesResponseType(typeof(BaseResponse<FamiliaDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<FamiliaDto>), 400)]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var response = await _mediator.Send(new DeleteFamiliaCommand { Id = id });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Devuelve por Id.</summary>
        [HttpGet("byId")]
        [ProducesResponseType(typeof(BaseResponse<FamiliaDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<FamiliaDto>), 400)]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var response = await _mediator.Send(new GetFamiliaByIdQuery { Id = id });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Busca por NombreFamilia.</summary>
        [HttpGet("search")]
        [ProducesResponseType(typeof(BaseResponse<List<FamiliaDto>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<List<FamiliaDto>>), 400)]
        public async Task<IActionResult> Search([FromQuery] string? nombreFamilia = null)
        {
            var response = await _mediator.Send(new SearchFamiliaQuery { NombreFamilia = nombreFamilia });
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
