using Domain.Dtos;
using MediatR;

namespace Application.UseCase.AjusteStockOperation.Command.DeleteAjusteStock
{
    public class DeleteAjusteStockCommand : IRequest<BaseResponse<AjusteStockDto>>
    {
        public int Id { get; set; }
    }
}
