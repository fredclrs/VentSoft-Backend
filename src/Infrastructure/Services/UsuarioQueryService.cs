using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class UsuarioQueryService : IUsuarioQueryService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UsuarioQueryService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UsuarioDto?> GetUserByIdAsync(int id)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
            return usuario == null ? null : _mapper.Map<UsuarioDto>(usuario);
        }

        public async Task<List<UsuarioDto>> SearchUsersAsync(string? nombre = null, string? documentoIdentidad = null, string? zona = null)
        {
            var query = _context.Usuarios.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nombre))
                query = query.Where(u => u.Nombre.Contains(nombre));

            if (!string.IsNullOrWhiteSpace(documentoIdentidad))
                query = query.Where(u => u.DocumentoIdentidad.Contains(documentoIdentidad));

            if (!string.IsNullOrWhiteSpace(zona))
                query = query.Where(u => u.Zona != null && u.Zona.Contains(zona));

            var usuarios = await query.ToListAsync();
            return _mapper.Map<List<UsuarioDto>>(usuarios);
        }
    }
}
