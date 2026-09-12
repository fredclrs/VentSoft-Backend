namespace Domain.Dtos
{
    /// <summary>Datos de entrada para registrar una Compra junto con sus renglones (DetalleCompra) en una sola operación.</summary>
    public class RegistrarCompraDto
    {
        public DateTime Fecha { get; set; }
        public string? Referencias { get; set; }
        public string? Nota { get; set; }

        public int IdUsuario { get; set; }
        public int IdProveedor { get; set; }

        /// <summary>Monto pagado al momento de registrar la compra (puede ser 0 = compra a crédito).</summary>
        public double Pagado { get; set; }

        public List<RegistrarDetalleCompraDto> Detalles { get; set; } = new();
    }
}
