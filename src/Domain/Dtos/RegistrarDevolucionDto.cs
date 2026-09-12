namespace Domain.Dtos
{
    /// <summary>
    /// Datos de entrada para registrar una devolución (o cambio, si trae ArticulosCambio)
    /// de artículos de una Venta ya registrada.
    /// </summary>
    public class RegistrarDevolucionDto
    {
        public int IdVenta { get; set; }
        public int IdUsuario { get; set; }
        public string? Motivo { get; set; }

        public List<RegistrarDetalleDevolucionDto> Detalles { get; set; } = new();

        /// <summary>Vacío si es una devolución pura (sin cambio).</summary>
        public List<RegistrarDetalleCambioDto> ArticulosCambio { get; set; } = new();

        /// <summary>Si el neto de la operación queda a favor del negocio (el cambio es más caro
        /// que lo devuelto), cuánto paga el cliente ahora mismo (el resto queda pendiente).</summary>
        public double MontoCobradoAhora { get; set; }

        /// <summary>Si el neto queda a favor del cliente, decide el cajero: true = se le devuelve en
        /// efectivo ahora, false = queda como saldo a favor para una compra futura.</summary>
        public bool DevolverEnEfectivo { get; set; }
    }
}
