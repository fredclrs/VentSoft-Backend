namespace Domain.Dtos
{
    public class FamiliaDto
    {
        public int Id { get; set; }
        public string NombreFamilia { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string Estado { get; set; } = null!;
    }
}
