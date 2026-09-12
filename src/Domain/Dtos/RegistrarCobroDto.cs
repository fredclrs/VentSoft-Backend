namespace Domain.Dtos
{
    /// <summary>Datos de entrada para registrar un cobro a un cliente. La deuda actual se calcula, no se envía.</summary>
    public class RegistrarCobroDto
    {
        public DateTime Fecha { get; set; }
        public double Monto { get; set; }
        public string? Recibo { get; set; }
        public string? Nota { get; set; }

        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }
    }
}
