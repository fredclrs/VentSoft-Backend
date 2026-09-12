using Domain.Dtos;
using MediatR;

namespace Application.UseCase.FormaDePagoOperation.Queries.GetFormaDePagoById
{
    public class GetFormaDePagoByIdQuery : IRequest<BaseResponse<FormaDePagoDto>>
    {
        public int Id { get; set; }
    }
}
