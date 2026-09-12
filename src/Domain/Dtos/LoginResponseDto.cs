namespace Domain.Dtos
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = null!;
        public DateTime Expiracion { get; set; }
        public UsuarioDto Usuario { get; set; } = null!;
    }
}
