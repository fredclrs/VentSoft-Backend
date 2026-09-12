using Application;
using Application.UseCase.UserOperation.Command.AddUser;
using Application.UseCase.UserOperation.Command.DeleteUSer;
using Application.UseCase.UserOperation.Command.UpdateUSer;
using Application.UseCase.UserOperation.Queries.GetUserById;
using Application.UseCase.UserOperation.Queries.SearchUsers;
using Domain.Common;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebVentSoft.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsuarioController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Crea un nuevo usuario.
        /// </summary>
        /// <param name="usuarioDto">Datos del usuario a crear.</param>
        /// <returns>Devuelve un BaseResponse con el usuario creado.</returns>
        /// <response code="200">Usuario creado correctamente.</response>
        /// <response code="400">Error al crear el usuario.</response>
        [HttpPost]
        [Authorize(Roles = Permisos.Usuarios)]
        [ProducesResponseType(typeof(BaseResponse<UsuarioDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<UsuarioDto>), 400)]
        public async Task<IActionResult> InsertUser([FromBody] UsuarioDto usuarioDto)
        {
            var response = await _mediator.Send(new AddUserCommand { UsuarioDto = usuarioDto });

            if (response.Success)
                return Ok(response);
            else
                return BadRequest(response);
        }

        /// <summary>
        /// Actualizar el usuario.
        /// </summary>
        /// <param name="id">Id del usuario a modificar.</param>
        /// <param name="usuarioDto">Datos del usuario a modificar.</param>
        /// <returns>Devuelve un BaseResponse con el usuario actualizado.</returns>
        /// <response code="200">Usuario actualizado correctamente.</response>
        /// <response code="400">Error al actualizar el usuario.</response>
        [HttpPut]
        [Authorize(Roles = Permisos.Usuarios)]
        [ProducesResponseType(typeof(BaseResponse<UsuarioDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<UsuarioDto>), 400)]
        public async Task<IActionResult> UpdateUser([FromQuery] int id, [FromBody] UsuarioDto usuarioDto)
        {
            var response = await _mediator.Send(new UpdateUserCommand { Id = id, UsuarioDto = usuarioDto });

            if (response.Success)
                return Ok(response);
            else
                return BadRequest(response);
        }

        /// <summary>
        /// Da de baja (lógica) al usuario seleccionado.
        /// </summary>
        /// <param name="id">Id del usuario a dar de baja</param>
        /// <returns>Devuelve un BaseResponse con el usuario dado de baja.</returns>
        /// <response code="200">Usuario dado de baja correctamente.</response>
        /// <response code="400">Error al dar de baja al usuario.</response>
        [HttpDelete]
        [Authorize(Roles = Permisos.Usuarios)]
        [ProducesResponseType(typeof(BaseResponse<UsuarioDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<UsuarioDto>), 400)]
        public async Task<IActionResult> DeleteUser([FromQuery] int id)
        {
            var response = await _mediator.Send(new DeleteUserCommand { Id = id });

            if (response.Success)
                return Ok(response);
            else
                return BadRequest(response);
        }

        /// <summary>
        /// Devuelve el usuario con el Id seleccionado.
        /// </summary>
        /// <param name="id">Id del usuario a buscar</param>
        /// <returns>Devuelve un BaseResponse con el usuario encontrado.</returns>
        /// <response code="200">Usuario devuelto correctamente.</response>
        /// <response code="400">Error al encontrar al usuario con el Id seleccionado.</response>
        [HttpGet("byId")]
        [ProducesResponseType(typeof(BaseResponse<UsuarioDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<UsuarioDto>), 400)]
        public async Task<IActionResult> GetUserById([FromQuery] int id)
        {
            var response = await _mediator.Send(new GetUserByIdQuery { Id = id });

            if (response.Success)
                return Ok(response);
            else
                return BadRequest(response);
        }

        /// <summary>
        /// Busca usuarios por nombre, documento de identidad y/o zona.
        /// </summary>
        /// <param name="nombre">nombre del usuario</param>
        /// <param name="documentoIdentidad">documento de identidad del usuario</param>
        /// <param name="zona">zona del usuario</param>
        /// <returns>Devuelve un BaseResponse con los usuarios encontrados.</returns>
        /// <response code="200">Usuarios devueltos correctamente.</response>
        /// <response code="400">Error al buscar usuarios.</response>
        [HttpGet("searchUser")]
        [ProducesResponseType(typeof(BaseResponse<List<UsuarioDto>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<List<UsuarioDto>>), 400)]
        public async Task<IActionResult> SearchUser(
            [FromQuery] string? nombre = null,
            [FromQuery] string? documentoIdentidad = null,
            [FromQuery] string? zona = null)
        {
            var response = await _mediator.Send(new SearchUserQuery { Nombre = nombre, DocumentoIdentidad = documentoIdentidad, Zona = zona });

            if (response.Success)
                return Ok(response);
            else
                return BadRequest(response);
        }
    }
}
