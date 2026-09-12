using AutoMapper;
using Domain.Dtos;
using Domain.Entities;

namespace Application.Mappings
{
    public class ConfiguracionEmpresaProfile : Profile
    {
        public ConfiguracionEmpresaProfile()
        {
            CreateMap<ConfiguracionEmpresa, ConfiguracionEmpresaDto>();
        }
    }
}
