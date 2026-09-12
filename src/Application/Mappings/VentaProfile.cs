using AutoMapper;
using Domain.Dtos;
using Domain.Entities;

namespace Application.Mappings
{
    public class VentaProfile : Profile
    {
        public VentaProfile()
        {
            CreateMap<Venta, VentaDto>();
            CreateMap<DetalleVenta, DetalleVentaDto>();
        }
    }
}
