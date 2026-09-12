namespace Domain.Entities
{
    /// <summary>Un artículo devuelto dentro de una DevolucionVenta.</summary>
    public class DetalleDevolucionVenta
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public double PrecioUnitario { get; set; }
        public double SubTotal { get; set; }

        /// <summary>Si es false, el artículo no vuelve a stock vendible (ej: dañado, sin etiqueta).</summary>
        public bool Vendible { get; set; } = true;

        public int IdDevolucion { get; set; }

        /// <summary>Renglón de la venta original del que proviene (siempre obligatorio).</summary>
        public int IdDetalleVenta { get; set; }
        public int IdArticulo { get; set; }

        public DevolucionVenta Devolucion { get; set; } = null!;
        public DetalleVenta DetalleVenta { get; set; } = null!;
        public Articulo Articulo { get; set; } = null!;
    }
}
