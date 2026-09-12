using Domain.Dtos;
using MediatR;

namespace Application.UseCase.FamiliaOperation.Command.AddFamilia
{
    public class AddFamiliaCommand : IRequest<BaseResponse<FamiliaDto>>
    {
        public FamiliaDto FamiliaDto { get; set; } = null!;
    }
}
