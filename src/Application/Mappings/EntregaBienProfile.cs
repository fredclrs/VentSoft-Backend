using AutoMapper;
using Domain.Dtos;
using Domain.Entities;

namespace Application.Mappings
{
    public class EntregaBienProfile : Profile
    {
        public EntregaBienProfile()
        {
            CreateMap<EntregaBien, EntregaBienDto>();
        }
    }
}
