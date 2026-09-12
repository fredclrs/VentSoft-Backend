using Domain.Dtos;
using MediatR;

namespace Application.UseCase.CobroOperation.Command.RegistrarCobro
{
    public class RegistrarCobroCommand : IRequest<BaseResponse<CobroDto>>
    {
        public RegistrarCobroDto CobroDto { get; set; } = null!;
    }
}
