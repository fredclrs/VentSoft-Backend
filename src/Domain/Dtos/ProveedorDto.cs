namespace Domain.Dtos
{
    public class ProveedorDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Nit { get; set; }
        public string? PersonaContacto { get; set; }
        public string? Direccion { get; set; }
        public string? Zona { get; set; }
        public int? Telefono { get; set; }
        public string? Correo { get; set; }
        public string? Nota { get; set; }
        public string Estado { get; set; } = null!;
    }
}
