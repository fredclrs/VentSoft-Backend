using Domain.Common;

namespace Domain.Entities
{
    /// <summary>
    /// Entrada o salida de efectivo de la caja que NO viene de una Venta ni de una Devolución/
    /// Cambio — por ejemplo, sacar plata para comprar algo puntual, un gasto suelto, o un
    /// aporte extra. Se usa únicamente para que el reporte "Ventas del día" pueda calcular bien
    /// cuánto efectivo debería quedar en la caja al cerrar el día.
    /// </summary>
    public class MovimientoCaja : AuditEntity
    {
        public DateTime Fecha { get; set; }

        /// <summary>"ENTRADA" o "SALIDA".</summary>
        public string Tipo { get; set; } = null!;

        /// <summary>Siempre positivo — el signo lo da Tipo, no este campo.</summary>
        public double Monto { get; set; }

        public string Motivo { get; set; } = null!;

        public int IdUsuario { get; set; }

        public Usuario Usuario { get; set; } = null!;
    }
}
