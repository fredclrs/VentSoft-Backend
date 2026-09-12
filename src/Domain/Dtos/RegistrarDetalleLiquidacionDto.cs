namespace Domain.Dtos
{
    /// <summary>Una boleta incluida en la liquidación, con el precio definitivo que se le fija
    /// en este momento (acá sí es obligatorio).</summary>
    public class RegistrarDetalleLiquidacionDto
    {
        public int IdEntrega { get; set; }
        public double PrecioUnitario { get; set; }
    }
}
