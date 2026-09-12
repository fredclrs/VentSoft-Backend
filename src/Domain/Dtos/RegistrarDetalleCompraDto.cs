namespace Domain.Dtos
{
    public class RegistrarDetalleCompraDto
    {
        public int IdArticulo { get; set; }
        public int Cantidad { get; set; }
        public decimal CostoUnitario { get; set; }

        /// <summary>Opcional: numero de lote del proveedor, si el negocio lo maneja.</summary>
        public string? Lote { get; set; }

        /// <summary>Opcional: vencimiento de este lote/compra. No todos los negocios lo necesitan.</summary>
        public DateTime? FechaVencimiento { get; set; }
    }
}
