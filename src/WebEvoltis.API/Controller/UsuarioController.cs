using Application;
using Application.UseCase.UserOperation.Command.AddUser;
using Application.UseCase.UserOperation.Command.DeleteUSer;
using Application.UseCase.UserOperation.Command.UpdateUSer;
using Application.UseCase.UserOperation.Queries.GetUserById;
using Application.UseCase.UserOperation.Queries.SearchUsers;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebEvoltis.API.Controller
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
        [ProducesResponseType(typeof(BaseResponse<UsuarioDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<UsuarioDto>), 400)]
        public async Task<IActionResult> InsertUser([FromBody] UsuarioDto usuarioDto)
        {       
            var response = await _mediator.Send(new AddUserCommand { UsuarioDto = usuarioDto });
      
            if (response.Success)
                return Ok(response); // HTTP 200 con el usuario creado y mensaje
            else
                return BadRequest(response); // HTTP 400 con mensaje de error
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
        [ProducesResponseType(typeof(BaseResponse<UsuarioDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<UsuarioDto>), 400)]
        public async Task<IActionResult> UpdateUser([FromQuery] int id,[FromBody] UsuarioDto usuarioDto)
        {
            var response = await _mediator.Send(new UpdateUserCommand {Id = id, UsuarioDto = usuarioDto });

            if (response.Success)
                return Ok(response); // HTTP 200 con el usuario creado y mensaje
            else
                return BadRequest(response); // HTTP 400 con mensaje de error
        }

        /// <summary>
        /// Borra el usuario seleccionado.
        /// </summary>
        /// <param name="id">Id del usuario a borrar</param>
        /// <returns>Devuelve un BaseResponse con el usuario eliminado.</returns>
        /// <response code="200">Usuario eliminado correctamente.</response>
        /// <response code="400">Error al eliminar el usuario.</response>
        [HttpDelete]
        [ProducesResponseType(typeof(BaseResponse<UsuarioDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<UsuarioDto>), 400)]
        public async Task<IActionResult> DeleteUser([FromQuery] int id)
        {
            var response = await _mediator.Send(new DeleteUserCommand { Id = id });

            if (response.Success)
                return Ok(response); // HTTP 200 con el usuario eliminado y mensaje
            else
                return BadRequest(response); // HTTP 400 con mensaje de error
        }

        /// <summary>
        /// Devuelve el usuario con el Id seleccionado.
        /// </summary>
        /// <param name="id">Id del usuario a borrar</param>
        /// <returns>Devuelve un BaseResponse con el usuario encontrado.</returns>
        /// <response code="200">Usuario devuelto correctamente.</response>
        /// <response code="400">Error al encontrar al usuario con el Id selecionado.</response>
        [HttpGet("byId")]
        [ProducesResponseType(typeof(BaseResponse<UsuarioDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<UsuarioDto>), 400)]
        public async Task<IActionResult> GetUserById([FromQuery] int id)
        {
            var response = await _mediator.Send(new GetUserByIdQuery { Id = id });

            if (response.Success)
                return Ok(response); // HTTP 200 con el usuario encontrado y mensaje
            else
                return BadRequest(response); // HTTP 400 con mensaje de error
        }

        /// <summary>
        /// Devuelve el usuario por nombre, ciudad, provincia.
        /// </summary> 
        /// <param name="nombre">nombre del usuario</param>
        /// <param name="ciudad">ciudad del usuario</param>
        /// <param name="provincia">provincia del usuario</param>
        /// <returns>Devuelve un BaseResponse con los usuarios encontrados.</returns>
        /// <response code="200">Usuario devuelto correctamente.</response>
        /// <response code="400">Error al encontrar al usuario con el Id selecionado.</response>
        [HttpGet("searchUser")]
        [ProducesResponseType(typeof(BaseResponse<UsuarioDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<UsuarioDto>), 400)]
        public async Task<IActionResult> SearchUser([FromQuery] string nombre = null,
                    [FromQuery] string ciudad = null,
                    [FromQuery] string provincia = null)
        {
            var response = await _mediator.Send(new SearchUserQuery { Nombre = nombre, Ciudad = ciudad, Provincia = provincia });

            if (response.Success)
                return Ok(response); // HTTP 200 con los usuarios encontrados y mensaje
            else
                return BadRequest(response); // HTTP 400 con mensaje de error
        }

    }
}
