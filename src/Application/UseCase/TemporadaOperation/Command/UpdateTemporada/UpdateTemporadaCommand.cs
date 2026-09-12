using Domain.Dtos;
using MediatR;

namespace Application.UseCase.TemporadaOperation.Command.UpdateTemporada
{
    public class UpdateTemporadaCommand : IRequest<BaseResponse<TemporadaDto>>
    {
        public int Id { get; set; }
        public TemporadaDto TemporadaDto { get; set; } = null!;
    }
}
