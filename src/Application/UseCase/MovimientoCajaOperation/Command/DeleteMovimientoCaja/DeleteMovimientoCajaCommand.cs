using Domain.Dtos;
using MediatR;

namespace Application.UseCase.MovimientoCajaOperation.Command.DeleteMovimientoCaja
{
    public class DeleteMovimientoCajaCommand : IRequest<BaseResponse<MovimientoCajaDto>>
    {
        public int Id { get; set; }
    }
}
