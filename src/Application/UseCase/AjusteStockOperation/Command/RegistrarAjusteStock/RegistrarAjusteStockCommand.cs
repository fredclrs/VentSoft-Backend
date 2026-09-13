using Domain.Dtos;
using MediatR;

namespace Application.UseCase.AjusteStockOperation.Command.RegistrarAjusteStock
{
    public class RegistrarAjusteStockCommand : IRequest<BaseResponse<AjusteStockDto>>
    {
        public RegistrarAjusteStockDto AjusteDto { get; set; } = null!;
    }
}
