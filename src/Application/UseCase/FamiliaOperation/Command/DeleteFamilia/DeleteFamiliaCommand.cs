using Domain.Dtos;
using MediatR;

namespace Application.UseCase.FamiliaOperation.Command.DeleteFamilia
{
    public class DeleteFamiliaCommand : IRequest<BaseResponse<FamiliaDto>>
    {
        public int Id { get; set; }
    }
}
