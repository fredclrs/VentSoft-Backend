using Domain.Dtos;
using MediatR;

namespace Application.UseCase.TemporadaOperation.Queries.GetTemporadaById
{
    public class GetTemporadaByIdQuery : IRequest<BaseResponse<TemporadaDto>>
    {
        public int Id { get; set; }
    }
}
