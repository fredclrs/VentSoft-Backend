namespace Domain.Dtos
{
    /// <summary>Datos para registrar una liquidación: junta una o más boletas pendientes de un
    /// cliente, les fija el precio y aplica el total contra su deuda general.</summary>
    public class RegistrarLiquidacionDto
    {
        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }
        public string? Nota { get; set; }

        public List<RegistrarDetalleLiquidacionDto> Entregas { get; set; } = new();

        /// <summary>Si el valor entregado supera la deuda, decide quien liquida: true = se
        /// devuelve en efectivo ahora, false = queda como saldo a favor.</summary>
        public bool DevolverEnEfectivo { get; set; }
    }
}
