using Domain.Common;

namespace Domain.Entities
{
    /// <summary>
    /// Tipo de atributo libre (p.ej. "Talla", "Color", "Material", "Lote",
    /// "Principio Activo"). El valor concreto para cada artículo vive en
    /// ArticuloCaracteristica.
    /// </summary>
    public class Caracteristica : AuditEntity
    {
        public string NombreCaracteristica { get; set; } = null!;
        public string? Descripcion { get; set; }

        public ICollection<ArticuloCaracteristica> Articulos { get; set; } = new List<ArticuloCaracteristica>();
    }
}
