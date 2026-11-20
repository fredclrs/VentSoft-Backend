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

        /// <summary>Promoción por cantidad, de uno de dos tipos según EsPrecioPorMayor:
        /// - false, "N por $X" (ej. Chicle 2x500): cada grupo de CantidadPromo unidades se cobra
        ///   PrecioPromo en total; las que sobran (llevar 3 en una 2x500) van a precio normal.
        /// - true, precio por mayor (ej. desde 3, cada una a 450): llevando CantidadPromo o más,
        ///   TODAS las unidades se cobran PrecioPromo cada una.
        /// Cantidad y precio en null = no es por cantidad. La aplica la pantalla de Ventas como
        /// descuento del renglón (DetalleVenta.DescuentoMonetario).</summary>
        public int? CantidadPromo { get; set; }
        public decimal? PrecioPromo { get; set; }
        public bool EsPrecioPorMayor { get; set; }

        public ICollection<Articulo> Articulos { get; set; } = new List<Articulo>();
        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
    }
}
