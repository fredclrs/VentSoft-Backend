
namespace Application.Interfaces
{
    /// <summary>
    /// Contrato genérico de CRUD que implementan todos los repositorios.
    /// Trabaja sobre entidades de dominio (no DTOs); el mapeo a DTO ocurre
    /// en la capa de Application/Infrastructure Services. Cuando una entidad
    /// necesite operaciones extra, se crea una interfaz específica que
    /// extienda esta (ver IUsuarioRepository, IArticuloRepository, IVentaRepository).
    /// </summary>
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();

        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);

        Task SaveChangesAsync();
    }
}
