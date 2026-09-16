namespace Domain.Dtos
{
    public class CompraDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string? Referencias { get; set; }
        public double Total { get; set; }
        public double Pagado { get; set; }
        public double PorPagar { get; set; }
        public string? Nota { get; set; }
        public string Estado { get; set; } = null!;

        public int IdUsuario { get; set; }
        public int IdProveedor { get; set; }

        public List<DetalleCompraDto> Detalles { get; set; } = new();

        /// <summary>Artículos con Margen de ganancia configurado cuyo precio de venta sugerido
        /// cambió en esta compra (todavía no aplicado — el cajero lo confirma o rechaza después
        /// de guardar). Vacío si ninguno cambió.</summary>
        public List<PrecioSugeridoDto> PreciosSugeridos { get; set; } = new();

        /// <summary>Artículos SIN Margen de ganancia configurado cuyo costo subió en esta
        /// compra — el sistema no puede recalcular el precio solo, así que solo avisa para que
        /// se revise a mano. Vacío si ninguno aplica.</summary>
        public List<AvisoSinMargenDto> AvisosSinMargen { get; set; } = new();
    }
}
