using MediatR;

namespace Application.UseCase.EtiquetaPendienteOperation.Command.EliminarEtiquetaPendiente
{
    public class EliminarEtiquetaPendienteCommand : IRequest<BaseResponse<bool>>
    {
        public int Id { get; set; }
    }
}
