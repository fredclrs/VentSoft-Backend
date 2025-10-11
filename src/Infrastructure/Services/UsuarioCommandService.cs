
using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    internal class UsuarioCommandService : IUsuarioCommandService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UsuarioCommandService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UsuarioDto> AddUserAsync(UsuarioDto usuarioDto)
        {
            var usuario = _mapper.Map<Usuario>(usuarioDto);
            usuario.FechaCreacion = DateTime.Now;

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return _mapper.Map<UsuarioDto>(usuario);
        }

        public async Task<UsuarioDto?> UpdateUserAsync(int id, UsuarioDto usuarioDto)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Domicilios)
                .FirstOrDefaultAsync(u => u.ID == id);

            if (usuario == null)
                return null;

            // Actualiza datos principales
            usuario.Nombre = usuarioDto.Nombre;
            usuario.Email = usuarioDto.Email;

            // Actualiza/agrega domicilios
            foreach (var domicilioDto in usuarioDto.Domicilios)
            {
                // Validación de campos requeridos antes de guardar
                if (string.IsNullOrWhiteSpace(domicilioDto.Calle) ||
                    string.IsNullOrWhiteSpace(domicilioDto.Ciudad) ||
                    string.IsNullOrWhiteSpace(domicilioDto.Provincia))
                {
                    throw new InvalidOperationException("Todos los campos del domicilio son requeridos.");
                }

                var domicilioExistente = usuario.Domicilios
                    .FirstOrDefault(d => d.ID == domicilioDto.Id);

                if (domicilioExistente != null)
                {
                    // Actualizar domicilio existente
                    domicilioExistente.Calle = domicilioDto.Calle;
                    domicilioExistente.Ciudad = domicilioDto.Ciudad;
                    domicilioExistente.Provincia = domicilioDto.Provincia;
                }
                else
                {
                    // Agregar nuevo domicilio
                    var nuevoDomicilio = _mapper.Map<Domicilio>(domicilioDto);
                    usuario.Domicilios.Add(nuevoDomicilio);
                }
            }            

            await _context.SaveChangesAsync();

            return _mapper.Map<UsuarioDto>(usuario);
        }

        public async Task<UsuarioDto?> DeleteUserAsync(int id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Domicilios)
                .FirstOrDefaultAsync(u => u.ID == id);

            if (usuario == null)
                return null;

            _context.Domicilios.RemoveRange(usuario.Domicilios);
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return _mapper.Map<UsuarioDto>(usuario);
        }
    }
}
