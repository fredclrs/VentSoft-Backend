using Domain.Dtos;
using MediatR;

namespace Application.UseCase.TemporadaOperation.Command.AddTemporada
{
    public class AddTemporadaCommand : IRequest<BaseResponse<TemporadaDto>>
    {
        public TemporadaDto TemporadaDto { get; set; } = null!;
    }
}
