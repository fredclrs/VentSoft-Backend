using Domain.Common;

namespace Domain.Entities
{
    public class Compra : AuditEntity
    {
        public DateTime Fecha { get; set; }
        public string? Referencias { get; set; }
        public double Total { get; set; }
        public double Pagado { get; set; }
        public double PorPagar { get; set; }
        public string? Nota { get; set; }

        /// <summary>Por qué se anuló (Estado = "AN"). Quién y cuándo quedan en UserBaja/FechaBaja.
        /// Una compra anulada no cuenta para stock ni deuda — ver AnularCompraCommand.</summary>
        public string? MotivoAnulacion { get; set; }

        public int IdUsuario { get; set; }
        public int IdProveedor { get; set; }

        public Usuario Usuario { get; set; } = null!;
        public Proveedor Proveedor { get; set; } = null!;

        public ICollection<DetalleCompra> Detalles { get; set; } = new List<DetalleCompra>();
    }
}
