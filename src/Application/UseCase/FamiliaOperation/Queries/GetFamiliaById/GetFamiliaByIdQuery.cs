using Domain.Dtos;
using MediatR;

namespace Application.UseCase.FamiliaOperation.Queries.GetFamiliaById
{
    public class GetFamiliaByIdQuery : IRequest<BaseResponse<FamiliaDto>>
    {
        public int Id { get; set; }
    }
}
