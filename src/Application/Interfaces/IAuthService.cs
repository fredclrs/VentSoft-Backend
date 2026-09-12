using Domain.Dtos;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        /// <summary>Valida las credenciales y devuelve el usuario si son correctas y está activo (Estado == "AC").</summary>
        Task<UsuarioDto?> ValidateUserAsync(string nombreUsuario, string contrasena);

        /// <summary>Genera el JWT para el usuario autenticado.</summary>
        (string Token, DateTime Expiracion) GenerateToken(UsuarioDto usuario);
    }
}
