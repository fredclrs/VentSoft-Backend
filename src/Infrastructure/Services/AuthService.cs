using Application.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Dtos;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IConfiguration _configuration;

        public AuthService(
            AppDbContext context,
            IMapper mapper,
            IPasswordHasher passwordHasher,
            IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        public async Task<UsuarioDto?> ValidateUserAsync(string nombreUsuario, string contrasena)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario && u.Estado == "AC");

            if (usuario == null)
                return null;

            if (!_passwordHasher.Verify(contrasena, usuario.Contrasena))
                return null;

            return _mapper.Map<UsuarioDto>(usuario);
        }

        public (string Token, DateTime Expiracion) GenerateToken(UsuarioDto usuario)
        {
            var jwtSection = _configuration.GetSection("Jwt");
            var key = jwtSection["Key"]
                ?? throw new InvalidOperationException("Falta configurar Jwt:Key en appsettings.json.");
            var issuer = jwtSection["Issuer"];
            var audience = jwtSection["Audience"];
            var expireMinutes = int.TryParse(jwtSection["ExpireMinutes"], out var minutos) ? minutos : 480;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, usuario.NombreUsuario),
                new Claim("nombre", usuario.Nombre),
                new Claim("esAdministrador", usuario.EsAdministrador ? "true" : "false"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // El Administrador tiene todos los permisos sin necesidad de tildar nada; el resto
            // de los usuarios, solo los que se les otorgó puntualmente (ver Domain.Common.Permisos).
            // Cada permiso viaja como un claim de rol separado: así [Authorize(Roles = "ventas")]
            // en cada controller ya funciona sin necesidad de políticas custom.
            var permisosDelUsuario = usuario.EsAdministrador
                ? Permisos.Todos
                : usuario.Permisos.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            foreach (var permiso in permisosDelUsuario)
                claims.Add(new Claim(ClaimTypes.Role, permiso));

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var expiracion = DateTime.UtcNow.AddMinutes(expireMinutes);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiracion,
                signingCredentials: credentials);

            return (new JwtSecurityTokenHandler().WriteToken(token), expiracion);
        }
    }
}
