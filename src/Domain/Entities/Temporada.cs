using Domain.Common;

namespace Domain.Entities
{
    /// <summary>
    /// Temporada o campaña de venta (ej: "Verano", "Invierno", "Campaña siembra"), definida
    /// por un rango de meses del año. Es una configuración 100% opcional: si un negocio no
    /// define ninguna, el sistema no se comporta distinto en nada (Ventas/Compras no la usan
    /// para nada al registrar); solo sirve para que el reporte "Ventas por temporada" pueda
    /// clasificar automáticamente cada venta según su fecha.
    /// </summary>
    public class Temporada : AuditEntity
    {
        public string Nombre { get; set; } = null!;

        /// <summary>Mes de inicio (1-12).</summary>
        public int MesInicio { get; set; }

        /// <summary>
        /// Mes de fin (1-12). Puede ser menor que MesInicio para rangos que cruzan fin de año
        /// (ej: Septiembre a Marzo => MesInicio=9, MesFin=3).
        /// </summary>
        public int MesFin { get; set; }
    }
}
