using Domain.Dtos;
using MediatR;

namespace Application.UseCase.ArticuloOperation.Queries.GetStockArticulo
{
    public class GetStockArticuloQuery : IRequest<BaseResponse<ArticuloStockDto>>
    {
        public int IdArticulo { get; set; }
    }
}
