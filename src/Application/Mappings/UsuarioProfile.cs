using AutoMapper;
using Domain.Dtos;
using Domain.Entities;

namespace Application.Mappings
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<Usuario, UsuarioDto>()
                .ForMember(d => d.Contrasena, opt => opt.Ignore()); // nunca se expone el hash en las respuestas

            CreateMap<UsuarioDto, Usuario>()
                .ForMember(e => e.Id, opt => opt.Ignore())
                .ForMember(e => e.Contrasena, opt => opt.Ignore())
                .ForMember(e => e.ConfirmarContrasena, opt => opt.Ignore()); // se setean ya hasheadas en UsuarioCommandService
        }
    }
}
