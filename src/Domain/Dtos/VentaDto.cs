namespace Domain.Dtos
{
    public class VentaDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }

        /// <summary>Momento real (fecha Y hora) en que se registró la venta — a diferencia de
        /// Fecha, que es solo la fecha "de negocio" (sin hora, y el cajero la puede elegir).
        /// Se usa para mostrar la hora real en el comprobante impreso.</summary>
        public DateTime? FechaRegistro { get; set; }

        public string? Referencias { get; set; }
        public double? DescuentoMonetario { get; set; }
        public double? DescuentoPorcentaje { get; set; }
        public double? RecargoPorcentaje { get; set; }
        public double Total { get; set; }
        public double Pagado { get; set; }
        public double PorPagar { get; set; }
        public string? Nota { get; set; }
        public double MontoSaldoAFavorAplicado { get; set; }
        public string Estado { get; set; } = null!;

        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }
        public int? IdPromocion { get; set; }
        public int? IdFormaDePago { get; set; }

        public List<DetalleVentaDto> Detalles { get; set; } = new();
    }
}
