using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class UsuarioCommandService : IUsuarioCommandService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;

        public UsuarioCommandService(AppDbContext context, IMapper mapper, IPasswordHasher passwordHasher)
        {
            _context = context;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<UsuarioDto> AddUserAsync(UsuarioDto usuarioDto)
        {
            var usuario = _mapper.Map<Usuario>(usuarioDto);

            var hash = _passwordHasher.Hash(usuarioDto.Contrasena ?? string.Empty);
            usuario.Contrasena = hash;
            // Columna heredada de la BD, redundante con Contrasena (ver notas del proyecto).
            usuario.ConfirmarContrasena = hash;

            usuario.Estado = "AC";
            usuario.FechaRegistro = DateTime.Now;

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return _mapper.Map<UsuarioDto>(usuario);
        }

        public async Task<UsuarioDto?> UpdateUserAsync(int id, UsuarioDto usuarioDto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
            if (usuario == null)
                return null;

            usuario.Nombre = usuarioDto.Nombre;
            usuario.DocumentoIdentidad = usuarioDto.DocumentoIdentidad;
            usuario.Nit = usuarioDto.Nit;
            usuario.Direccion = usuarioDto.Direccion;
            usuario.Zona = usuarioDto.Zona;
            usuario.Telefono = usuarioDto.Telefono;
            usuario.Correo = usuarioDto.Correo;
            usuario.Nota = usuarioDto.Nota;
            usuario.NombreUsuario = usuarioDto.NombreUsuario;
            usuario.EsAdministrador = usuarioDto.EsAdministrador;
            usuario.Permisos = usuarioDto.Permisos;

            if (!string.IsNullOrWhiteSpace(usuarioDto.Contrasena))
            {
                var hash = _passwordHasher.Hash(usuarioDto.Contrasena);
                usuario.Contrasena = hash;
                usuario.ConfirmarContrasena = hash;
            }

            // TODO: reemplazar "system" por el usuario autenticado real cuando exista login.
            usuario.UserActualizado = "system";
            usuario.FechaActualizado = DateTime.Now;

            await _context.SaveChangesAsync();

            return _mapper.Map<UsuarioDto>(usuario);
        }

        public async Task<UsuarioDto?> DeleteUserAsync(int id)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
            if (usuario == null)
                return null;

            // Baja lógica, no física: el usuario puede tener Compras/Ventas/Cobros/Pagos asociados.
            usuario.Estado = "IN";
            usuario.UserBaja = "system"; // TODO: usuario autenticado real
            usuario.FechaBaja = DateTime.Now;

            await _context.SaveChangesAsync();

            return _mapper.Map<UsuarioDto>(usuario);
        }
    }
}
