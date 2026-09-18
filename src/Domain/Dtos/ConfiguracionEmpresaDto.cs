namespace Domain.Dtos
{
    public class ConfiguracionEmpresaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Moneda { get; set; } = null!;
        public bool PermiteVentaACredito { get; set; }
        public bool PermiteCompraACredito { get; set; }
        public bool RedondearPreciosEnteros { get; set; }
        public bool PermiteCodigoCompartidoEntreArticulos { get; set; }
        public int? IdClientePorDefecto { get; set; }
        public int? IdProveedorPorDefecto { get; set; }

        /// <summary>Datos completos del cliente/proveedor por defecto, para preseleccionarlo
        /// directamente en Ventas/Compras sin una consulta extra. Null si no hay uno configurado.</summary>
        public ClienteDto? ClientePorDefecto { get; set; }
        public ProveedorDto? ProveedorPorDefecto { get; set; }
    }
}
