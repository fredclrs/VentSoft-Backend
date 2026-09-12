namespace Domain.Dtos
{
    public class CaracteristicaDto
    {
        public int Id { get; set; }
        public string NombreCaracteristica { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string Estado { get; set; } = null!;
    }
}
