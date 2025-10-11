using Domain.Dtos;

namespace Application.Interfaces
{
    public interface IUsuarioCommandService
    {
        Task<UsuarioDto> AddUserAsync(UsuarioDto usuarioDto);
        Task<UsuarioDto?> UpdateUserAsync(int id, UsuarioDto usuarioDto);
        Task<UsuarioDto?> DeleteUserAsync(int id);
    }
}
