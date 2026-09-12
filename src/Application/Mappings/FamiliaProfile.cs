using AutoMapper;
using Domain.Dtos;
using Domain.Entities;

namespace Application.Mappings
{
    public class FamiliaProfile : Profile
    {
        public FamiliaProfile()
        {
            CreateMap<Familia, FamiliaDto>();
            CreateMap<FamiliaDto, Familia>()
                .ForMember(e => e.Id, opt => opt.Ignore())
                .ForMember(e => e.Estado, opt => opt.Ignore())
                .ForMember(e => e.UserRegistro, opt => opt.Ignore())
                .ForMember(e => e.UserActualizado, opt => opt.Ignore())
                .ForMember(e => e.UserBaja, opt => opt.Ignore())
                .ForMember(e => e.FechaRegistro, opt => opt.Ignore())
                .ForMember(e => e.FechaActualizado, opt => opt.Ignore())
                .ForMember(e => e.FechaBaja, opt => opt.Ignore());
        }
    }
}
