using Domain.Common;

namespace Domain.Entities
{
    /// <summary>
    /// Devolución (y, opcionalmente, cambio) de artículos de una Venta ya registrada.
    /// Siempre referencia una Venta existente (no se permiten devoluciones "libres").
    ///
    /// Flujo de dinero al registrarla (ver RegistrarDevolucionCommandHandler): el Total de la
    /// Venta se recalcula sacando TotalDevuelto y sumando TotalCambio (0 si es una devolución
    /// pura) EN UN SOLO paso — la deuda que la Venta ya tenía por el artículo devuelto sigue
    /// siendo válida para cubrir el artículo nuevo, no se "perdona" para volver a cobrarlo desde
    /// cero. Sobre ese nuevo Total:
    /// - Si el cliente ya había pagado de más: sobra plata, se le devuelve en efectivo ahora
    ///   (MontoDevueltoEfectivo) o se acredita como saldo a favor (SaldoAFavorGenerado, ver
    ///   Cliente.SaldoAFavor) — lo elige el cajero.
    /// - Si la nueva deuda de la Venta quedó por encima de la que tenía antes de esta operación
    ///   (se llevó algo de más valor que lo devuelto): esa diferencia extra (nunca el valor
    ///   completo del artículo nuevo) se cobra ahora (MontoCobradoAhora) y/o queda pendiente
    ///   (PorPagar, informativo — ya está reflejado en Venta.PorPagar, no se vuelve a sumar).
    /// </summary>
    public class DevolucionVenta : AuditEntity
    {
        public DateTime Fecha { get; set; }
        public string? Motivo { get; set; }

        public int IdVenta { get; set; }
        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }

        public double TotalDevuelto { get; set; }
        public double TotalCambio { get; set; }

        public double AplicadoADeudaVenta { get; set; }
        public double MontoCobradoAhora { get; set; }

        /// <summary>Cuánto de la diferencia a favor del negocio quedó pendiente al registrar esta
        /// operación — informativo: ya está incluido en Venta.PorPagar, no sumar aparte.</summary>
        public double PorPagar { get; set; }
        public double MontoDevueltoEfectivo { get; set; }
        public double SaldoAFavorGenerado { get; set; }

        public Venta Venta { get; set; } = null!;
        public Cliente Cliente { get; set; } = null!;
        public Usuario Usuario { get; set; } = null!;

        /// <summary>Artículos que el cliente devuelve.</summary>
        public ICollection<DetalleDevolucionVenta> Detalles { get; set; } = new List<DetalleDevolucionVenta>();

        /// <summary>Artículos nuevos que se lleva a cambio (vacío si es una devolución pura, sin cambio).</summary>
        public ICollection<DetalleCambioVenta> ArticulosCambio { get; set; } = new List<DetalleCambioVenta>();
    }
}
