using Domain.Common;

namespace Domain.Entities
{
    public class Cobro : AuditEntity
    {
        public double DeudaActual { get; set; }
        public DateTime Fecha { get; set; }
        public double Monto { get; set; }
        public string? Recibo { get; set; }
        public string? Nota { get; set; }

        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }

        public Cliente Cliente { get; set; } = null!;
        public Usuario Usuario { get; set; } = null!;
    }
}
