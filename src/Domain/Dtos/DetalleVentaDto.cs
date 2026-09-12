namespace Domain.Dtos
{
    public class DetalleVentaDto
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public double PrecioUnitario { get; set; }

        /// <summary>Costo del artículo al momento de esta venta (null en ventas viejas, de
        /// antes de este campo). Ver comentario en Domain.Entities.DetalleVenta.</summary>
        public double? CostoUnitario { get; set; }

        public double? DescuentoMonetario { get; set; }
        public double? DescuentoPorcentaje { get; set; }
        public double SubTotal { get; set; }
        public double Pagado { get; set; }

        public int IdVenta { get; set; }
        public int IdArticulo { get; set; }
    }
}
