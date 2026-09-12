using Domain.Common;

namespace Domain.Entities
{
    public class Proveedor : AuditEntity
    {
        public string Nombre { get; set; } = null!;
        public string? Nit { get; set; }
        public string? PersonaContacto { get; set; }
        public string? Direccion { get; set; }
        public string? Zona { get; set; }
        public int? Telefono { get; set; }
        public string? Correo { get; set; }
        public string? Nota { get; set; }

        public ICollection<Compra> Compras { get; set; } = new List<Compra>();
        public ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    }
}
