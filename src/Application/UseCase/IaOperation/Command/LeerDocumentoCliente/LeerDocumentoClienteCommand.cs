using Domain.Dtos;
using MediatR;

namespace Application.UseCase.IaOperation.Command.LeerDocumentoCliente
{
    public class LeerDocumentoClienteCommand : IRequest<BaseResponse<DatosClienteExtraidosDto>>
    {
        public byte[] ImagenBytes { get; set; } = null!;
        public string MediaType { get; set; } = null!;
    }
}
