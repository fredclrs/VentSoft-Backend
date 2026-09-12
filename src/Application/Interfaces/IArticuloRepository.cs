
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IArticuloRepository : IRepository<Articulo>
{
    Task<Articulo?> GetByCodigoAsync(string codigo);

    /// <summary>Trae el artículo con sus atributos (Caracteristicas) y el nombre de cada uno.</summary>
    Task<Articulo?> GetByIdWithCaracteristicasAsync(int id);

    /// <summary>Trae todos los artículos con sus atributos, para listados/búsquedas.</summary>
    Task<List<Articulo>> GetAllWithCaracteristicasAsync();
}
