using Domain.Common;

namespace Domain.Entities
{
    public class Venta : AuditEntity
    {
        public DateTime Fecha { get; set; }
        public string? Referencias { get; set; }
        public double? DescuentoMonetario { get; set; }
        public double? DescuentoPorcentaje { get; set; }
        public double Total { get; set; }
        public double Pagado { get; set; }
        public double PorPagar { get; set; }
        public string? Nota { get; set; }

        /// <summary>Cuánto de lo Pagado vino del SaldoAFavor del cliente (crédito por una
        /// devolución/cambio anterior) en vez de efectivo — solo para trazabilidad/impresión.</summary>
        public double MontoSaldoAFavorAplicado { get; set; }

        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }

        /// <summary>Opcional: no toda venta tiene una promoción aplicada.</summary>
        public int? IdPromocion { get; set; }

        public Cliente Cliente { get; set; } = null!;
        public Usuario Usuario { get; set; } = null!;
        public Promocion? Promocion { get; set; }

        public ICollection<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();
        public ICollection<DevolucionVenta> Devoluciones { get; set; } = new List<DevolucionVenta>();
    }
}
