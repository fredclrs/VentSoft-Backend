using Domain.Common;

namespace Domain.Entities
{
    /// <summary>
    /// Corrección manual de stock que no viene de una Compra ni de una Venta — rotura, vencimiento,
    /// robo, o simplemente un conteo real distinto al que el sistema tenía calculado. Se suma (o
    /// resta, según Tipo) a la fórmula de stock (ver StockService), igual que hacen Compra/Venta.
    /// </summary>
    public class AjusteStock : AuditEntity
    {
        public DateTime Fecha { get; set; }

        /// <summary>"ENTRADA" (se encontró más stock del esperado, corrección al alza) o "SALIDA"
        /// (rotura, vencimiento, robo, corrección a la baja) — mismo patrón que MovimientoCaja.</summary>
        public string Tipo { get; set; } = null!;

        /// <summary>Siempre positivo — el signo lo da Tipo, no este campo.</summary>
        public int Cantidad { get; set; }

        /// <summary>Por qué se ajusta — obligatorio, es lo que le da sentido al ajuste (sin esto
        /// es un cambio de stock sin explicación, tan malo como el problema que resuelve).</summary>
        public string Motivo { get; set; } = null!;

        public int IdArticulo { get; set; }
        public int IdUsuario { get; set; }

        public Articulo Articulo { get; set; } = null!;
        public Usuario Usuario { get; set; } = null!;
    }
}
