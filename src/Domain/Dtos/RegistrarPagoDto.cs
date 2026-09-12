namespace Domain.Dtos
{
    /// <summary>Datos de entrada para registrar un pago a un proveedor. La deuda actual se calcula, no se envía.</summary>
    public class RegistrarPagoDto
    {
        public DateTime Fecha { get; set; }
        public double Monto { get; set; }
        public string? Recibo { get; set; }
        public string? Nota { get; set; }

        public int IdProveedor { get; set; }
        public int IdUsuario { get; set; }
    }
}
