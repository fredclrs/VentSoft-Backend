using Application;
using Application.UseCase.ClienteOperation.Command.AddCliente;
using Application.UseCase.ClienteOperation.Command.DeleteCliente;
using Application.UseCase.ClienteOperation.Command.UpdateCliente;
using Application.UseCase.ClienteOperation.Queries.GetClienteById;
using Application.UseCase.ClienteOperation.Queries.SearchClientes;
using Domain.Common;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebVentSoft.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClienteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>Crea un nuevo cliente.</summary>
        [HttpPost]
        [Authorize(Roles = Permisos.Ventas)]
        [ProducesResponseType(typeof(BaseResponse<ClienteDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<ClienteDto>), 400)]
        public async Task<IActionResult> Insert([FromBody] ClienteDto clienteDto)
        {
            var response = await _mediator.Send(new AddClienteCommand { ClienteDto = clienteDto });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Actualiza un cliente existente.</summary>
        [HttpPut]
        [Authorize(Roles = Permisos.Ventas)]
        [ProducesResponseType(typeof(BaseResponse<ClienteDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<ClienteDto>), 400)]
        public async Task<IActionResult> Update([FromQuery] int id, [FromBody] ClienteDto clienteDto)
        {
            var response = await _mediator.Send(new UpdateClienteCommand { Id = id, ClienteDto = clienteDto });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Da de baja (lógica) a un cliente.</summary>
        [HttpDelete]
        [Authorize(Roles = Permisos.Ventas)]
        [ProducesResponseType(typeof(BaseResponse<ClienteDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<ClienteDto>), 400)]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var response = await _mediator.Send(new DeleteClienteCommand { Id = id });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Devuelve un cliente por Id.</summary>
        [HttpGet("byId")]
        [ProducesResponseType(typeof(BaseResponse<ClienteDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<ClienteDto>), 400)]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var response = await _mediator.Send(new GetClienteByIdQuery { Id = id });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Busca clientes por nombre y/o documento de identidad.</summary>
        [HttpGet("search")]
        [ProducesResponseType(typeof(BaseResponse<List<ClienteDto>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<List<ClienteDto>>), 400)]
        public async Task<IActionResult> Search([FromQuery] string? nombre = null, [FromQuery] string? documentoIdentidad = null)
        {
            var response = await _mediator.Send(new SearchClienteQuery { Nombre = nombre, DocumentoIdentidad = documentoIdentidad });
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
