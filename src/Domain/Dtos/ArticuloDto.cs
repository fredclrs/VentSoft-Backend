namespace Domain.Dtos
{
    public class ArticuloDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string Tamano { get; set; } = null!;
        public string? UnidadMedida { get; set; }
        public double Fraccion { get; set; }
        public double Precio { get; set; }
        public double Costo { get; set; }
        public double? PrecioUnidadSuelta { get; set; }
        public double? MargenGanancia { get; set; }
        public double? StockMinimo { get; set; }
        public double? StockIdeal { get; set; }
        public string? Imagen { get; set; }
        public string Estado { get; set; } = null!;

        public int IdFamilia { get; set; }
        public int? IdPromocion { get; set; }

        public List<ArticuloCaracteristicaDto> Caracteristicas { get; set; } = new();
    }
}
