namespace Domain.Dtos
{
    /// <summary>Un renglon de compra (lote) con fecha de vencimiento cargada, para negocios que la usan.</summary>
    public class LoteVencimientoDto
    {
        public int IdDetalleCompra { get; set; }
        public int IdArticulo { get; set; }
        public string CodigoArticulo { get; set; } = null!;
        public string? DescripcionArticulo { get; set; }
        public string? Lote { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public int Cantidad { get; set; }
        public int IdCompra { get; set; }
        public DateTime FechaCompra { get; set; }
    }
}
