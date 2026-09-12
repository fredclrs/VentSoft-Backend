using Domain.Common;

namespace Domain.Entities
{
    public class Familia : AuditEntity
    {
        public string NombreFamilia { get; set; } = null!;
        public string? Descripcion { get; set; }

        public ICollection<Articulo> Articulos { get; set; } = new List<Articulo>();
    }
}
