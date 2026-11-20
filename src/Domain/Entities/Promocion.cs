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

        /// <summary>Promoción "N por $X" (ej. Chicle 2x500): llevando CantidadPromo unidades se
        /// cobra PrecioPromo en total; las que sobran (llevar 3 en una 2x500) van a precio normal.
        /// Las dos en null = no es de este tipo. La aplica la pantalla de Ventas como descuento
        /// del renglón (DetalleVenta.DescuentoMonetario).</summary>
        public int? CantidadPromo { get; set; }
        public decimal? PrecioPromo { get; set; }

        public ICollection<Articulo> Articulos { get; set; } = new List<Articulo>();
        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
    }
}
