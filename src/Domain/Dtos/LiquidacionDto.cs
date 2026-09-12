namespace Domain.Dtos
{
    public class LiquidacionDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string? Nota { get; set; }
        public string Estado { get; set; } = null!;

        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }

        public double DeudaAntes { get; set; }
        public double TotalEntregado { get; set; }
        public double DeudaActual { get; set; }
        public double MontoDevueltoEfectivo { get; set; }
        public double SaldoAFavorGenerado { get; set; }

        public List<EntregaBienDto> Entregas { get; set; } = new();
    }
}
