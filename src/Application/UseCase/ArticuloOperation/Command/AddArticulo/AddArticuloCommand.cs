using Domain.Dtos;
using MediatR;

namespace Application.UseCase.ArticuloOperation.Command.AddArticulo
{
    public class AddArticuloCommand : IRequest<BaseResponse<ArticuloDto>>
    {
        public ArticuloDto ArticuloDto { get; set; } = null!;
    }
}
