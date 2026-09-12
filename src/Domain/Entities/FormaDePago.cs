using Domain.Common;

namespace Domain.Entities
{
    /// <summary>
    /// Nota: en la BD actual esta tabla existe pero ninguna otra tabla la referencia
    /// (Venta no tiene columna IdFormaPago todavía). Ver notas del proyecto.
    /// </summary>
    public class FormaDePago : AuditEntity
    {
        /// <summary>Columna "FormaPago" en la BD.</summary>
        public string Nombre { get; set; } = null!;
    }
}
