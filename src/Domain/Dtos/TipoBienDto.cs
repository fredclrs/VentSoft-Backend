namespace Domain.Dtos
{
    public class TipoBienDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string UnidadMedida { get; set; } = null!;
        public string Estado { get; set; } = null!;
    }
}
