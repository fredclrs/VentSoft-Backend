using AutoMapper;
using Domain.Dtos;
using Domain.Entities;

namespace Application.Mappings
{
    public class EtiquetaPendienteProfile : Profile
    {
        public EtiquetaPendienteProfile()
        {
            CreateMap<EtiquetaPendiente, EtiquetaPendienteDto>();
        }
    }
}
