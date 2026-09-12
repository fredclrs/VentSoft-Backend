using AutoMapper;
using Domain.Dtos;
using Domain.Entities;

namespace Application.Mappings
{
    public class LiquidacionProfile : Profile
    {
        public LiquidacionProfile()
        {
            CreateMap<Liquidacion, LiquidacionDto>();
        }
    }
}
