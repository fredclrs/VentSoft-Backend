using AutoMapper;
using Domain.Dtos;
using Domain.Entities;

namespace Application.Mappings
{
    public class MovimientoCajaProfile : Profile
    {
        public MovimientoCajaProfile()
        {
            CreateMap<MovimientoCaja, MovimientoCajaDto>();
        }
    }
}
