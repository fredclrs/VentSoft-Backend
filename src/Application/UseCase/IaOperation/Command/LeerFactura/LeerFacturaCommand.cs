using Domain.Dtos;
using MediatR;

namespace Application.UseCase.IaOperation.Command.LeerFactura
{
    public class LeerFacturaCommand : IRequest<BaseResponse<List<LineaFacturaDto>>>
    {
        public byte[] ImagenBytes { get; set; } = null!;
        public string MediaType { get; set; } = null!;
    }
}
