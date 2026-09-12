namespace Domain.Dtos
{
    public class TemporadaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public int MesInicio { get; set; }
        public int MesFin { get; set; }
        public string Estado { get; set; } = null!;
    }
}
