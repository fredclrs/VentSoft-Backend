using Domain.Common;

namespace Domain.Entities
{
    /// <summary>
    /// Cierre de cuenta con pago en especie: junta una o más EntregaBien pendientes de un
    /// cliente, les fija el precio definitivo y aplica el valor total contra la deuda general
    /// del cliente (todas sus Ventas con saldo pendiente, más antiguas primero — igual que un
    /// Cobro en efectivo). Si el valor entregado supera la deuda, el sobrante se devuelve en
    /// efectivo o se acredita como saldo a favor (ver Cliente.SaldoAFavor), a elección de quien
    /// liquida.
    /// </summary>
    public class Liquidacion : AuditEntity
    {
        public DateTime Fecha { get; set; }
        public string? Nota { get; set; }

        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }

        public double DeudaAntes { get; set; }
        public double TotalEntregado { get; set; }

        /// <summary>Deuda que le queda al cliente después de aplicar esta liquidación (0 si
        /// alcanzó o sobró).</summary>
        public double DeudaActual { get; set; }

        public double MontoDevueltoEfectivo { get; set; }
        public double SaldoAFavorGenerado { get; set; }

        public Cliente Cliente { get; set; } = null!;
        public Usuario Usuario { get; set; } = null!;

        /// <summary>Boletas incluidas en esta liquidación.</summary>
        public ICollection<EntregaBien> Entregas { get; set; } = new List<EntregaBien>();
    }
}
