using Domain.Dtos;
using MediatR;

namespace Application.UseCase.EtiquetaPendienteOperation.Command.AgregarEtiquetaPendiente
{
    public class AgregarEtiquetaPendienteCommand : IRequest<BaseResponse<EtiquetaPendienteDto>>
    {
        public AgregarEtiquetaPendienteDto EtiquetaDto { get; set; } = null!;
    }
}
