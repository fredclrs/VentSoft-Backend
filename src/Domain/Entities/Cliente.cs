using Domain.Common;

namespace Domain.Entities
{
    public class Cliente : AuditEntity
    {
        public string Nombre { get; set; } = null!;
        public string DocumentoIdentidad { get; set; } = null!;
        public string? PersonaContacto { get; set; }
        public string? Direccion { get; set; }
        public string? Zona { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public string? Nota { get; set; }

        /// <summary>Crédito acumulado del cliente por devoluciones/cambios no devueltos en efectivo
        /// en el momento — se puede aplicar como pago en una futura venta.</summary>
        public double SaldoAFavor { get; set; }

        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
        public ICollection<Cobro> Cobros { get; set; } = new List<Cobro>();
        public ICollection<DevolucionVenta> Devoluciones { get; set; } = new List<DevolucionVenta>();
        public ICollection<EntregaBien> EntregasBien { get; set; } = new List<EntregaBien>();
        public ICollection<Liquidacion> Liquidaciones { get; set; } = new List<Liquidacion>();
    }
}
