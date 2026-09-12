namespace Domain.Dtos
{
    public class EntregaBienDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string? NumeroBoleta { get; set; }
        public double Cantidad { get; set; }
        public double? PrecioUnitario { get; set; }
        public double? SubTotal { get; set; }
        public string? Nota { get; set; }
        public string Estado { get; set; } = null!;

        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }
        public int IdTipoBien { get; set; }
        public int? IdLiquidacion { get; set; }
    }
}
