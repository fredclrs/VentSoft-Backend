using AutoMapper;
using Domain.Dtos;
using Domain.Entities;

namespace Application.Mappings
{
    public class DevolucionVentaProfile : Profile
    {
        public DevolucionVentaProfile()
        {
            CreateMap<DevolucionVenta, DevolucionVentaDto>();
            CreateMap<DetalleDevolucionVenta, DetalleDevolucionVentaDto>();
            CreateMap<DetalleCambioVenta, DetalleCambioVentaDto>();
        }
    }
}
