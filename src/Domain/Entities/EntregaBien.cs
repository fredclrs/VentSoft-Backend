using Domain.Common;

namespace Domain.Entities
{
    /// <summary>
    /// Una "boleta" de entrega de un bien (ej: grano) que un cliente trae para pagar su deuda en
    /// especie, en vez de efectivo. El precio es OPCIONAL al registrarla — en muchos rubros
    /// (agro) el precio del bien se define recién más adelante — y se vuelve obligatorio al
    /// incluirla en una Liquidacion, que es donde realmente impacta la deuda del cliente.
    /// Mientras IdLiquidacion sea null, la entrega está pendiente y no afecta nada todavía.
    /// </summary>
    public class EntregaBien : AuditEntity
    {
        public DateTime Fecha { get; set; }

        /// <summary>Número de boleta física que trae el cliente, para trazabilidad (opcional).</summary>
        public string? NumeroBoleta { get; set; }

        public double Cantidad { get; set; }

        /// <summary>Nulo hasta que se liquida: recién ahí se fija el precio definitivo.</summary>
        public double? PrecioUnitario { get; set; }
        public double? SubTotal { get; set; }

        public string? Nota { get; set; }

        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }
        public int IdTipoBien { get; set; }

        /// <summary>Null mientras está pendiente; se completa al incluirla en una Liquidacion.</summary>
        public int? IdLiquidacion { get; set; }

        public Cliente Cliente { get; set; } = null!;
        public Usuario Usuario { get; set; } = null!;
        public TipoBien TipoBien { get; set; } = null!;
        public Liquidacion? Liquidacion { get; set; }
    }
}
