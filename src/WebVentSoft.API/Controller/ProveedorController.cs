using Application;
using Application.UseCase.ProveedorOperation.Command.AddProveedor;
using Application.UseCase.ProveedorOperation.Command.DeleteProveedor;
using Application.UseCase.ProveedorOperation.Command.UpdateProveedor;
using Application.UseCase.ProveedorOperation.Queries.GetProveedorById;
using Application.UseCase.ProveedorOperation.Queries.SearchProveedores;
using Domain.Common;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebVentSoft.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProveedorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>Crea un nuevo proveedor.</summary>
        [HttpPost]
        [Authorize(Roles = Permisos.Compras)]
        [ProducesResponseType(typeof(BaseResponse<ProveedorDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<ProveedorDto>), 400)]
        public async Task<IActionResult> Insert([FromBody] ProveedorDto proveedorDto)
        {
            var response = await _mediator.Send(new AddProveedorCommand { ProveedorDto = proveedorDto });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Actualiza un proveedor existente.</summary>
        [HttpPut]
        [Authorize(Roles = Permisos.Compras)]
        [ProducesResponseType(typeof(BaseResponse<ProveedorDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<ProveedorDto>), 400)]
        public async Task<IActionResult> Update([FromQuery] int id, [FromBody] ProveedorDto proveedorDto)
        {
            var response = await _mediator.Send(new UpdateProveedorCommand { Id = id, ProveedorDto = proveedorDto });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Da de baja (lógica) a un proveedor.</summary>
        [HttpDelete]
        [Authorize(Roles = Permisos.Compras)]
        [ProducesResponseType(typeof(BaseResponse<ProveedorDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<ProveedorDto>), 400)]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var response = await _mediator.Send(new DeleteProveedorCommand { Id = id });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Devuelve un proveedor por Id.</summary>
        [HttpGet("byId")]
        [ProducesResponseType(typeof(BaseResponse<ProveedorDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<ProveedorDto>), 400)]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var response = await _mediator.Send(new GetProveedorByIdQuery { Id = id });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Busca proveedores por nombre y/o NIT.</summary>
        [HttpGet("search")]
        [ProducesResponseType(typeof(BaseResponse<List<ProveedorDto>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<List<ProveedorDto>>), 400)]
        public async Task<IActionResult> Search([FromQuery] string? nombre = null, [FromQuery] string? nit = null)
        {
            var response = await _mediator.Send(new SearchProveedorQuery { Nombre = nombre, Nit = nit });
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
