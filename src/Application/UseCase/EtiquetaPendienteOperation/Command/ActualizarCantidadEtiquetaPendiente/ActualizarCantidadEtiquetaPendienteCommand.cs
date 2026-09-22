using Domain.Dtos;
using MediatR;

namespace Application.UseCase.EtiquetaPendienteOperation.Command.ActualizarCantidadEtiquetaPendiente
{
    public class ActualizarCantidadEtiquetaPendienteCommand : IRequest<BaseResponse<EtiquetaPendienteDto>>
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
    }
}
