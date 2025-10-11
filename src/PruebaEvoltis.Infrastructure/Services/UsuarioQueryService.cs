
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
        public async Task<UsuarioDto> GetUserByIdAsync(int id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Domicilios)
                .FirstOrDefaultAsync(u => u.ID == id);

            return _mapper.Map<UsuarioDto>(usuario);
        }

        public async Task<List<UsuarioDto>> SearchUsersAsync(string nombre = null, string ciudad = null, string provincia = null)
        {
            // Inicia la consulta incluyendo domicilios
            var query = _context.Usuarios
                                .Include(u => u.Domicilios)
                                .AsQueryable();

            // Filtrar por nombre si se pasó
            if (!string.IsNullOrWhiteSpace(nombre))
            {
                query = query.Where(u => u.Nombre.Contains(nombre));
            }

            // Filtrar por ciudad si se pasó
            if (!string.IsNullOrWhiteSpace(ciudad))
            {
                query = query.Where(u => u.Domicilios.Any(d => d.Ciudad.Contains(ciudad)));
            }

            // Filtrar por provincia si se pasó
            if (!string.IsNullOrWhiteSpace(provincia))
            {
                query = query.Where(u => u.Domicilios.Any(d => d.Provincia.Contains(provincia)));
            }

            var usuarios = await query.ToListAsync();

            // Mapear a DTO incluyendo domicilios si existen
            return _mapper.Map<List<UsuarioDto>>(usuarios);
        }
    }
}
