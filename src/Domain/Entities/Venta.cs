using Domain.Common;

namespace Domain.Entities
{
    public class Venta : AuditEntity
    {
        public DateTime Fecha { get; set; }
        public string? Referencias { get; set; }
        public double? DescuentoMonetario { get; set; }
        public double? DescuentoPorcentaje { get; set; }

        /// <summary>Recargo (%) aplicado sobre el Total por la forma de pago elegida (ej.
        /// Transferencia). Se sugiere solo desde FormaDePago.PorcentajeRecargo, pero queda
        /// guardado acá el valor real usado en ESTA venta puntual (pudo haberse ajustado a
        /// mano) — para trazabilidad y para que el comprobante lo pueda mostrar. Null = sin
        /// recargo, como la gran mayoría de las ventas.</summary>
        public double? RecargoPorcentaje { get; set; }
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

        /// <summary>Cómo se cobró lo de Pagado (Efectivo, Tarjeta, QR, etc. — catálogo
        /// configurable por negocio en FormaDePago). Opcional: null en ventas 100% a crédito
        /// (Pagado = 0, no hay ningún cobro que clasificar) o en ventas viejas de antes de este
        /// campo. El reporte "Ventas del día" la usa para separar el efectivo real de caja del
        /// resto (tarjeta/QR/transferencia).</summary>
        public int? IdFormaDePago { get; set; }

        public Cliente Cliente { get; set; } = null!;
        public Usuario Usuario { get; set; } = null!;
        public Promocion? Promocion { get; set; }
        public FormaDePago? FormaDePago { get; set; }

        public ICollection<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();
        public ICollection<DevolucionVenta> Devoluciones { get; set; } = new List<DevolucionVenta>();
    }
}
