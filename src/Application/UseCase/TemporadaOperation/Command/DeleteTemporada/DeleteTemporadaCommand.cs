using Domain.Dtos;
using MediatR;

namespace Application.UseCase.TemporadaOperation.Command.DeleteTemporada
{
    public class DeleteTemporadaCommand : IRequest<BaseResponse<TemporadaDto>>
    {
        public int Id { get; set; }
    }
}
