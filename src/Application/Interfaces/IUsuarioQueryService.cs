using Domain.Dtos;

namespace Application.Interfaces
{
    public interface IUsuarioQueryService
    {
        Task<List<UsuarioDto>> SearchUsersAsync(string nombre = null, string ciudad = null, string provincia = null);
        Task<UsuarioDto> GetUserByIdAsync(int id);
    }
}
