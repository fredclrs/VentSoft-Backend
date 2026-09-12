using Application;
using Application.UseCase.AuthOperation.Command.Login;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebVentSoft.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Inicia sesión con nombre de usuario y contraseña y devuelve un JWT.
        /// </summary>
        /// <param name="loginRequestDto">Credenciales del usuario.</param>
        /// <returns>Devuelve un BaseResponse con el token y los datos del usuario.</returns>
        /// <response code="200">Inicio de sesión correcto.</response>
        /// <response code="400">Credenciales inválidas.</response>
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(BaseResponse<LoginResponseDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<LoginResponseDto>), 400)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var response = await _mediator.Send(new LoginCommand { LoginRequestDto = loginRequestDto });

            if (response.Success)
                return Ok(response);
            else
                return BadRequest(response);
        }
    }
}
