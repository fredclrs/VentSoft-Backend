namespace Domain.Common
{
    /// <summary>
    /// Campos de auditoría comunes a la mayoría de las tablas de VentSoft:
    /// alta/baja lógica por Estado, y quién/cuándo hizo cada cambio.
    /// </summary>
    public abstract class AuditEntity
    {
        public int Id { get; set; }

        /// <summary>Estado del registro (p.ej. "AC" = Activo, "IN" = Inactivo/Baja).</summary>
        public string Estado { get; set; } = "AC";

        public string? UserRegistro { get; set; }
        public string? UserActualizado { get; set; }
        public string? UserBaja { get; set; }

        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaActualizado { get; set; }
        public DateTime? FechaBaja { get; set; }
    }
}
