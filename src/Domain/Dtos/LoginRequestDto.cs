namespace Domain.Dtos
{
    public class LoginRequestDto
    {
        public string NombreUsuario { get; set; } = null!;
        public string Contrasena { get; set; } = null!;
    }
}
