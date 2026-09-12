namespace Domain.Dtos
{
    public class ArticuloStockDto
    {
        public int IdArticulo { get; set; }
        public string Codigo { get; set; } = null!;
        public string? Descripcion { get; set; }
        public int StockActual { get; set; }
        public double? StockMinimo { get; set; }
        public double? StockIdeal { get; set; }
    }
}
