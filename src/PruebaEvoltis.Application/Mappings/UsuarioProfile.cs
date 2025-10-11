using AutoMapper;
using Domain.Dtos;
using Domain.Entities;

namespace Application.Mappings
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<Usuario, UsuarioDto>().ReverseMap();
            CreateMap<Domicilio, DomicilioDto>().ReverseMap();
        }
    }
  
}
