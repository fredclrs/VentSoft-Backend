using AutoMapper;
using Domain.Dtos;
using Domain.Entities;

namespace Application.Mappings
{
    public class CompraProfile : Profile
    {
        public CompraProfile()
        {
            CreateMap<Compra, CompraDto>();
            CreateMap<DetalleCompra, DetalleCompraDto>();
        }
    }
}
