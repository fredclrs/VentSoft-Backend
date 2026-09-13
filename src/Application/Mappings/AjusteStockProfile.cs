using AutoMapper;
using Domain.Dtos;
using Domain.Entities;

namespace Application.Mappings
{
    public class AjusteStockProfile : Profile
    {
        public AjusteStockProfile()
        {
            CreateMap<AjusteStock, AjusteStockDto>();
        }
    }
}
