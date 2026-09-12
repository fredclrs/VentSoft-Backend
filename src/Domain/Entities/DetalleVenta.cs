namespace Domain.Entities
{
    public class DetalleVenta
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public double PrecioUnitario { get; set; }

        /// <summary>Costo del artículo (Articulo.Costo) copiado en el instante exacto de esta
        /// venta — así la ganancia de este renglón queda congelada para siempre, sin importar
        /// que el costo del artículo cambie después por compras futuras a otro precio. Null en
        /// ventas registradas antes de agregar este campo: para esas, los reportes caen de
        /// vuelta al costo ACTUAL del artículo como aproximación (es lo único disponible).</summary>
        public double? CostoUnitario { get; set; }

        public double? DescuentoMonetario { get; set; }
        public double? DescuentoPorcentaje { get; set; }
        public double SubTotal { get; set; }
        public double Pagado { get; set; }

        public int IdVenta { get; set; }
        public int IdArticulo { get; set; }

        public Venta Venta { get; set; } = null!;
        public Articulo Articulo { get; set; } = null!;

        public ICollection<DetalleDevolucionVenta> Devoluciones { get; set; } = new List<DetalleDevolucionVenta>();
    }
}
