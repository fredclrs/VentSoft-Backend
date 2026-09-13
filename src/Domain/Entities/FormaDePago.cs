using Domain.Common;

namespace Domain.Entities
{
    /// <summary>
    /// Catálogo configurable por negocio (Efectivo, Tarjeta, QR, Transferencia, etc.) — Venta
    /// referencia una de estas por IdFormaDePago.
    /// </summary>
    public class FormaDePago : AuditEntity
    {
        /// <summary>Columna "FormaPago" en la BD.</summary>
        public string Nombre { get; set; } = null!;

        /// <summary>Marca cuál de las formas de pago del negocio cuenta como efectivo físico en
        /// la caja — el reporte "Ventas del día" la usa para separar "Efectivo en caja" del resto
        /// (tarjeta/QR/transferencia, que no es plata física). El negocio puede marcar más de una
        /// si le hace falta (ej. "Efectivo USD" y "Efectivo Bs." separados, ambos físicos).</summary>
        public bool EsEfectivo { get; set; } = false;
    }
}
