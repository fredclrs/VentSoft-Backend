namespace Domain.Entities
{
    /// <summary>Un artículo nuevo entregado al cliente como parte de un cambio (ver DevolucionVenta).</summary>
    public class DetalleCambioVenta
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public double PrecioUnitario { get; set; }
        public double SubTotal { get; set; }

        public int IdDevolucion { get; set; }
        public int IdArticulo { get; set; }

        public DevolucionVenta Devolucion { get; set; } = null!;
        public Articulo Articulo { get; set; } = null!;
    }
}
