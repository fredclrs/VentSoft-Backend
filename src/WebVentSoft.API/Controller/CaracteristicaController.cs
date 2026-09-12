using Application;
using Application.UseCase.CaracteristicaOperation.Command.AddCaracteristica;
using Application.UseCase.CaracteristicaOperation.Command.DeleteCaracteristica;
using Application.UseCase.CaracteristicaOperation.Command.UpdateCaracteristica;
using Application.UseCase.CaracteristicaOperation.Queries.GetCaracteristicaById;
using Application.UseCase.CaracteristicaOperation.Queries.SearchCaracteristicas;
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
    public class CaracteristicaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CaracteristicaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>Crea una nueva característica.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<CaracteristicaDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<CaracteristicaDto>), 400)]
        public async Task<IActionResult> Insert([FromBody] CaracteristicaDto dto)
        {
            var response = await _mediator.Send(new AddCaracteristicaCommand { CaracteristicaDto = dto });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Actualiza característica existente.</summary>
        [HttpPut]
        [ProducesResponseType(typeof(BaseResponse<CaracteristicaDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<CaracteristicaDto>), 400)]
        public async Task<IActionResult> Update([FromQuery] int id, [FromBody] CaracteristicaDto dto)
        {
            var response = await _mediator.Send(new UpdateCaracteristicaCommand { Id = id, CaracteristicaDto = dto });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Da de baja (lógica).</summary>
        [HttpDelete]
        [ProducesResponseType(typeof(BaseResponse<CaracteristicaDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<CaracteristicaDto>), 400)]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var response = await _mediator.Send(new DeleteCaracteristicaCommand { Id = id });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Devuelve por Id.</summary>
        [HttpGet("byId")]
        [ProducesResponseType(typeof(BaseResponse<CaracteristicaDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<CaracteristicaDto>), 400)]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var response = await _mediator.Send(new GetCaracteristicaByIdQuery { Id = id });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Busca por NombreCaracteristica.</summary>
        [HttpGet("search")]
        [ProducesResponseType(typeof(BaseResponse<List<CaracteristicaDto>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<List<CaracteristicaDto>>), 400)]
        public async Task<IActionResult> Search([FromQuery] string? nombreCaracteristica = null)
        {
            var response = await _mediator.Send(new SearchCaracteristicaQuery { NombreCaracteristica = nombreCaracteristica });
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
