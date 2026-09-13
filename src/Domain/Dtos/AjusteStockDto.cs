namespace Domain.Dtos
{
    public class AjusteStockDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; } = null!;
        public int Cantidad { get; set; }
        public string Motivo { get; set; } = null!;
        public string Estado { get; set; } = null!;
        public int IdArticulo { get; set; }
        public int IdUsuario { get; set; }
    }
}
