namespace Domain.Dtos
{
    public class DetalleCambioVentaDto
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public double PrecioUnitario { get; set; }
        public double SubTotal { get; set; }

        public int IdDevolucion { get; set; }
        public int IdArticulo { get; set; }
    }
}
