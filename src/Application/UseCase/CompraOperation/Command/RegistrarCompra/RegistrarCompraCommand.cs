using Domain.Dtos;
using MediatR;

namespace Application.UseCase.CompraOperation.Command.RegistrarCompra
{
    public class RegistrarCompraCommand : IRequest<BaseResponse<CompraDto>>
    {
        public RegistrarCompraDto CompraDto { get; set; } = null!;
    }
}
