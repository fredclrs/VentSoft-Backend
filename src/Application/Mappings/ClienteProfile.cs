using AutoMapper;
using Domain.Dtos;
using Domain.Entities;

namespace Application.Mappings
{
    public class ClienteProfile : Profile
    {
        public ClienteProfile()
        {
            CreateMap<Cliente, ClienteDto>();
            CreateMap<ClienteDto, Cliente>()
                .ForMember(e => e.Id, opt => opt.Ignore())
                .ForMember(e => e.Estado, opt => opt.Ignore())
                // El saldo a favor lo maneja únicamente el flujo de devoluciones/ventas (ver
                // RegistrarDevolucionCommandHandler / RegistrarVentaCommandHandler): nunca se
                // pisa desde el formulario de alta/edición de Cliente.
                .ForMember(e => e.SaldoAFavor, opt => opt.Ignore())
                .ForMember(e => e.UserRegistro, opt => opt.Ignore())
                .ForMember(e => e.UserActualizado, opt => opt.Ignore())
                .ForMember(e => e.UserBaja, opt => opt.Ignore())
                .ForMember(e => e.FechaRegistro, opt => opt.Ignore())
                .ForMember(e => e.FechaActualizado, opt => opt.Ignore())
                .ForMember(e => e.FechaBaja, opt => opt.Ignore());
        }
    }
}
