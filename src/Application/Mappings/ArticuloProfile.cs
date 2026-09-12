using AutoMapper;
using Domain.Dtos;
using Domain.Entities;

namespace Application.Mappings
{
    public class ArticuloProfile : Profile
    {
        public ArticuloProfile()
        {
            CreateMap<Articulo, ArticuloDto>();
            CreateMap<ArticuloDto, Articulo>()
                .ForMember(e => e.Id, opt => opt.Ignore())
                .ForMember(e => e.Estado, opt => opt.Ignore())
                .ForMember(e => e.UserRegistro, opt => opt.Ignore())
                .ForMember(e => e.UserActualizado, opt => opt.Ignore())
                .ForMember(e => e.UserBaja, opt => opt.Ignore())
                .ForMember(e => e.FechaRegistro, opt => opt.Ignore())
                .ForMember(e => e.FechaActualizado, opt => opt.Ignore())
                .ForMember(e => e.FechaBaja, opt => opt.Ignore())
                .ForMember(e => e.Familia, opt => opt.Ignore())
                .ForMember(e => e.Promocion, opt => opt.Ignore())
                // Las caracteristicas se manejan aparte en el handler (reemplazo completo del set).
                .ForMember(e => e.Caracteristicas, opt => opt.Ignore());

            CreateMap<ArticuloCaracteristica, ArticuloCaracteristicaDto>()
                .ForMember(d => d.NombreCaracteristica,
                    opt => opt.MapFrom(s => s.Caracteristica != null ? s.Caracteristica.NombreCaracteristica : null));

            CreateMap<ArticuloCaracteristicaDto, ArticuloCaracteristica>()
                .ForMember(e => e.Id, opt => opt.Ignore())
                .ForMember(e => e.IdArticulo, opt => opt.Ignore())
                .ForMember(e => e.Articulo, opt => opt.Ignore())
                .ForMember(e => e.Caracteristica, opt => opt.Ignore());
        }
    }
}
