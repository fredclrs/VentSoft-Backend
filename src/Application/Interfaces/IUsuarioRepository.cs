
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<Usuario?> GetByUsernameAsync(string nombreUsuario);
    }
}
