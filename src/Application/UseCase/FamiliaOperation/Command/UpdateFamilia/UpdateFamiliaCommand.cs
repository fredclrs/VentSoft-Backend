using Domain.Dtos;
using MediatR;

namespace Application.UseCase.FamiliaOperation.Command.UpdateFamilia
{
    public class UpdateFamiliaCommand : IRequest<BaseResponse<FamiliaDto>>
    {
        public int Id { get; set; }
        public FamiliaDto FamiliaDto { get; set; } = null!;
    }
}
