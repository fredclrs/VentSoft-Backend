namespace Domain.Dtos
{
    /// <summary>Informativo: un artículo SIN Margen de ganancia configurado cuyo Costo subió en
    /// esta Compra. Como no hay % cargado, el sistema no puede recalcular un precio de venta
    /// solo — esto solo avisa al cajero para que revise el precio a mano (ver
    /// RegistrarCompraCommandHandler).</summary>
    public class AvisoSinMargenDto
    {
        public int IdArticulo { get; set; }
        public string Codigo { get; set; } = null!;
        public double PrecioActual { get; set; }
        public double CostoAnterior { get; set; }
        public double CostoNuevo { get; set; }
    }
}
