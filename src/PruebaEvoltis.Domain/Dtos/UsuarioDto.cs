namespace Domain.Dtos
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<DomicilioDto> Domicilios { get; set; }

    }
}
