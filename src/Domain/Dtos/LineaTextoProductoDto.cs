namespace Domain.Dtos
{
    /// <summary>Un renglón que la IA interpretó de una lista de texto libre escrita a mano (ej.
    /// "remera polo azul 4"), ya comparado contra el catálogo — igual que LineaFacturaDto, es
    /// solo una sugerencia para revisar y confirmar.</summary>
    public class LineaTextoProductoDto
    {
        public string Descripcion { get; set; } = null!;
        public string? Talla { get; set; }
        public string? Color { get; set; }
        public int Cantidad { get; set; }

        public bool EsNuevo { get; set; }
        public int? IdArticuloExistente { get; set; }
        public string? CodigoExistente { get; set; }
    }
}
