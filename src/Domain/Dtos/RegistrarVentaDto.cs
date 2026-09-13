namespace Domain.Dtos
{
    /// <summary>Datos de entrada para registrar una Venta junto con sus renglones (DetalleVenta) en una sola operación.</summary>
    public class RegistrarVentaDto
    {
        public DateTime Fecha { get; set; }
        public string? Referencias { get; set; }

        /// <summary>Descuento adicional a nivel de venta completa (además de los descuentos por renglón, si los hubiera).</summary>
        public double? DescuentoMonetario { get; set; }
        public double? DescuentoPorcentaje { get; set; }

        public string? Nota { get; set; }

        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }

        /// <summary>Opcional: no toda venta tiene una promoción aplicada.</summary>
        public int? IdPromocion { get; set; }

        /// <summary>Monto pagado al momento de registrar la venta (puede ser 0 = venta a crédito/fiado).</summary>
        public double Pagado { get; set; }

        /// <summary>Cuánto del saldo a favor del cliente (por una devolución/cambio anterior) se
        /// aplica como pago de esta venta. 0 si no tiene o no se usa.</summary>
        public double MontoSaldoAFavorAplicado { get; set; }

        /// <summary>Cómo se cobró lo de Pagado (Efectivo, Tarjeta, QR, etc.). Opcional — no tiene
        /// sentido pedirlo si Pagado es 0 (venta 100% a crédito, no hay ningún cobro que
        /// clasificar).</summary>
        public int? IdFormaDePago { get; set; }

        public List<RegistrarDetalleVentaDto> Detalles { get; set; } = new();
    }
}
