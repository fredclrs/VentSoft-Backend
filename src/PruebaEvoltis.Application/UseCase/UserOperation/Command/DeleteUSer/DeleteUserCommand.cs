
using Domain.Dtos;
using MediatR;

namespace Application.UseCase.UserOperation.Command.DeleteUSer
{
    public class DeleteUserCommand: IRequest<BaseResponse<UsuarioDto>>
    {
        public int Id { get; set; }
    }
}
