namespace Domain.Dtos
{
    /// <summary>Informativo: un artículo con Margen de ganancia configurado cuyo Costo cambió en
    /// esta Compra, con el precio de venta que le correspondería según ese margen. Todavía NO
    /// se aplicó — el cajero lo confirma o lo rechaza después de guardar (ver
    /// ActualizarPrecioArticuloCommand).</summary>
    public class PrecioSugeridoDto
    {
        public int IdArticulo { get; set; }
        public string Codigo { get; set; } = null!;
        public double PrecioActual { get; set; }
        public double PrecioSugerido { get; set; }
    }
}
