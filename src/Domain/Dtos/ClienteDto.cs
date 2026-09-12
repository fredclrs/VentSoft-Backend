namespace Domain.Dtos
{
    public class ClienteDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string DocumentoIdentidad { get; set; } = null!;
        public string? PersonaContacto { get; set; }
        public string? Direccion { get; set; }
        public string? Zona { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public string? Nota { get; set; }
        public double SaldoAFavor { get; set; }
        public string Estado { get; set; } = null!;
    }
}
