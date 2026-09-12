namespace Domain.Dtos
{
    public class FormaDePagoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Estado { get; set; } = null!;
    }
}
