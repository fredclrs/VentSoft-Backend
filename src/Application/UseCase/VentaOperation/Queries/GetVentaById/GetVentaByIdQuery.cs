using Domain.Dtos;
using MediatR;

namespace Application.UseCase.VentaOperation.Queries.GetVentaById
{
    public class GetVentaByIdQuery : IRequest<BaseResponse<VentaDto>>
    {
        public int Id { get; set; }
    }
}
