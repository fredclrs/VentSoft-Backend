namespace Domain.Dtos
{
    public class PromocionDto
    {
        public int Id { get; set; }
        public string NombrePromocion { get; set; } = null!;
        public decimal? DescuentoMonetario { get; set; }
        public double? DescuentoPorcentaje { get; set; }
        public string? Descripcion { get; set; }
        public string Estado { get; set; } = null!;
    }
}
