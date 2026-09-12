using AutoMapper;
using Domain.Dtos;
using Domain.Entities;

namespace Application.Mappings
{
    public class CobroProfile : Profile
    {
        public CobroProfile()
        {
            CreateMap<Cobro, CobroDto>();
        }
    }
}
