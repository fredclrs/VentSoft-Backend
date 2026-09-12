namespace Domain.Dtos
{
    public class CobroDto
    {
        public int Id { get; set; }
        public double DeudaActual { get; set; }
        public DateTime Fecha { get; set; }
        public double Monto { get; set; }
        public string? Recibo { get; set; }
        public string? Nota { get; set; }
        public string Estado { get; set; } = null!;

        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }
    }
}
