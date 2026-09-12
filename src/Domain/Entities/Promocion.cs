using Domain.Common;

namespace Domain.Entities
{
    public class Promocion : AuditEntity
    {
        public string NombrePromocion { get; set; } = null!;
        public decimal? DescuentoMonetario { get; set; }

        /// <summary>Columna "DescuentoProcentaje" (con typo) en la BD.</summary>
        public double? DescuentoPorcentaje { get; set; }
        public string? Descripcion { get; set; }

        public ICollection<Articulo> Articulos { get; set; } = new List<Articulo>();
        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
    }
}
