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

        public int IdUsuario { get; set; }
        public int IdProveedor { get; set; }

        public Usuario Usuario { get; set; } = null!;
        public Proveedor Proveedor { get; set; } = null!;

        public ICollection<DetalleCompra> Detalles { get; set; } = new List<DetalleCompra>();
    }
}
