
using Domain.Dtos;
using MediatR;

namespace Application.UseCase.UserOperation.Queries.GetUserById
{
    public class GetUserByIdQuery: IRequest<BaseResponse<UsuarioDto>>
    {
        public int Id { get; set; }
    }
}
