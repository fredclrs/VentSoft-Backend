
using Domain.Dtos;

namespace Application.Interfaces
{
    public interface IUsuarioQueryService
    {
        Task<List<UsuarioDto>> SearchUsersAsync(string? nombre = null, string? documentoIdentidad = null, string? zona = null);
        Task<UsuarioDto?> GetUserByIdAsync(int id);
    }
}
