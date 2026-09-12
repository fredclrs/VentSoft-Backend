namespace Domain.Dtos
{
    /// <summary>Un artículo nuevo que el cliente se lleva a cambio (solo aplica a un Cambio, no a una devolución pura).</summary>
    public class RegistrarDetalleCambioDto
    {
        public int IdArticulo { get; set; }
        public int Cantidad { get; set; }
        public double PrecioUnitario { get; set; }
    }
}
