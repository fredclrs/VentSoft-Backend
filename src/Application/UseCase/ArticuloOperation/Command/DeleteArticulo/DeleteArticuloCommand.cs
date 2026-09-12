using Domain.Dtos;
using MediatR;

namespace Application.UseCase.ArticuloOperation.Command.DeleteArticulo
{
    public class DeleteArticuloCommand : IRequest<BaseResponse<ArticuloDto>>
    {
        public int Id { get; set; }
    }
}
