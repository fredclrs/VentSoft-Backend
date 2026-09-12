namespace Domain.Dtos
{
    public class DevolucionVentaDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string? Motivo { get; set; }
        public string Estado { get; set; } = null!;

        public int IdVenta { get; set; }
        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }

        public double TotalDevuelto { get; set; }
        public double TotalCambio { get; set; }
        public double AplicadoADeudaVenta { get; set; }
        public double MontoCobradoAhora { get; set; }
        public double PorPagar { get; set; }
        public double MontoDevueltoEfectivo { get; set; }
        public double SaldoAFavorGenerado { get; set; }

        public List<DetalleDevolucionVentaDto> Detalles { get; set; } = new();
        public List<DetalleCambioVentaDto> ArticulosCambio { get; set; } = new();
    }
}
