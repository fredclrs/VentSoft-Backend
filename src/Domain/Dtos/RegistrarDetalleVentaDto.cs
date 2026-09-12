namespace Domain.Dtos
{
    public class RegistrarDetalleVentaDto
    {
        public int IdArticulo { get; set; }
        public int Cantidad { get; set; }
        public double PrecioUnitario { get; set; }
        public double? DescuentoMonetario { get; set; }
        public double? DescuentoPorcentaje { get; set; }
    }
}
