
using Domain.Dtos;
using MediatR;

namespace Application.UseCase.UserOperation.Command.UpdateUSer
{
    public class UpdateUserCommand: IRequest<BaseResponse<UsuarioDto>>
    {
        public int Id { get; set; }
        public UsuarioDto UsuarioDto { get; set; }
    }
}
