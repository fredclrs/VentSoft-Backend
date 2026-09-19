
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IArticuloRepository : IRepository<Articulo>
{
    Task<Articulo?> GetByCodigoAsync(string codigo);

    /// <summary>Todos los artículos (activos o no) con ese código — con
    /// PermiteCodigoCompartidoEntreArticulos puede haber más de uno (una variante de talla/color
    /// cada uno). Se usa para chequear que no se repita la MISMA variante bajo el mismo código.</summary>
    Task<List<Articulo>> GetAllByCodigoAsync(string codigo);

    /// <summary>Trae el artículo con sus atributos (Caracteristicas) y el nombre de cada uno.</summary>
    Task<Articulo?> GetByIdWithCaracteristicasAsync(int id);

    /// <summary>Trae todos los artículos con sus atributos, para listados/búsquedas.</summary>
    Task<List<Articulo>> GetAllWithCaracteristicasAsync();
}
