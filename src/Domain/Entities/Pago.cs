using Domain.Common;

namespace Domain.Entities
{
    public class Pago : AuditEntity
    {
        public double DeudaActual { get; set; }
        public DateTime Fecha { get; set; }
        public double Monto { get; set; }
        public string? Recibo { get; set; }
        public string? Nota { get; set; }

        public int IdProveedor { get; set; }
        public int IdUsuario { get; set; }

        public Proveedor Proveedor { get; set; } = null!;
        public Usuario Usuario { get; set; } = null!;
    }
}
