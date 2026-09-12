using Domain.Common;

namespace Domain.Entities
{
    /// <summary>Catálogo de tipos de bien que un negocio puede aceptar como pago en especie de
    /// una deuda (ej: "Soja", "Trigo", "Maíz") — ver EntregaBien y Liquidacion.</summary>
    public class TipoBien : AuditEntity
    {
        public string Nombre { get; set; } = null!;

        /// <summary>Unidad en la que se mide (ej: "Toneladas", "Quintales", "Kg").</summary>
        public string UnidadMedida { get; set; } = null!;

        public ICollection<EntregaBien> Entregas { get; set; } = new List<EntregaBien>();
    }
}
