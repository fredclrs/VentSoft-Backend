using Domain.Dtos;
using MediatR;

namespace Application.UseCase.ArticuloOperation.Command.UpdateArticulo
{
    public class UpdateArticuloCommand : IRequest<BaseResponse<ArticuloDto>>
    {
        public int Id { get; set; }
        public ArticuloDto ArticuloDto { get; set; } = null!;
    }
}
