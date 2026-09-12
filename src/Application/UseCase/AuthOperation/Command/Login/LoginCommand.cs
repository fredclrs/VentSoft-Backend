using Domain.Dtos;
using MediatR;

namespace Application.UseCase.AuthOperation.Command.Login
{
    public class LoginCommand : IRequest<BaseResponse<LoginResponseDto>>
    {
        public LoginRequestDto? LoginRequestDto { get; set; }
    }
}
