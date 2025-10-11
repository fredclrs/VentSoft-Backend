using Domain.Dtos;
using MediatR;

namespace Application.UseCase.UserOperation.Command.AddUser
{
    public class AddUserCommand: IRequest<BaseResponse<UsuarioDto>>
    {
        public UsuarioDto? UsuarioDto { get; set; }
    }
}
