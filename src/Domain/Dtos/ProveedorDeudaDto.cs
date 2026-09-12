namespace Domain.Dtos
{
    public class ProveedorDeudaDto
    {
        public int IdProveedor { get; set; }
        public string NombreProveedor { get; set; } = null!;
        public double DeudaActual { get; set; }
    }
}
