using Domain.Dtos;
using MediatR;

namespace Application.UseCase.EntregaBienOperation.Command.RegistrarEntregaBien
{
    public class RegistrarEntregaBienCommand : IRequest<BaseResponse<EntregaBienDto>>
    {
        public RegistrarEntregaBienDto EntregaDto { get; set; } = null!;
    }
}
