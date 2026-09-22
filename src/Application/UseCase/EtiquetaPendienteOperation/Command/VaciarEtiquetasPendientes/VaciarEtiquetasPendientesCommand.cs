using MediatR;

namespace Application.UseCase.EtiquetaPendienteOperation.Command.VaciarEtiquetasPendientes
{
    /// <summary>Vacía toda la cola de una vez — se usa después de mandar a imprimir todo.</summary>
    public class VaciarEtiquetasPendientesCommand : IRequest<BaseResponse<int>>
    {
    }
}
