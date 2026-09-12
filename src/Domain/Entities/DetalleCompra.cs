namespace Domain.Entities
{
    public class DetalleCompra
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public decimal CostoUnitario { get; set; }

        /// <summary>Columna "SubtoTotal" (con typo) en la BD.</summary>
        public decimal SubTotal { get; set; }
        public decimal Pagado { get; set; }

        /// <summary>Opcional: numero de lote del proveedor, si el negocio lo maneja (p.ej. farmacia).</summary>
        public string? Lote { get; set; }

        /// <summary>Opcional: vencimiento de este lote/compra. No todos los negocios lo necesitan.</summary>
        public DateTime? FechaVencimiento { get; set; }

        public int IdCompra { get; set; }
        public int IdArticulo { get; set; }

        public Compra Compra { get; set; } = null!;
        public Articulo Articulo { get; set; } = null!;
    }
}
