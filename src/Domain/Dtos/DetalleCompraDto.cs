namespace Domain.Dtos
{
    public class DetalleCompraDto
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public decimal CostoUnitario { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Pagado { get; set; }
        public string? Lote { get; set; }
        public DateTime? FechaVencimiento { get; set; }

        public int IdCompra { get; set; }
        public int IdArticulo { get; set; }
    }
}
