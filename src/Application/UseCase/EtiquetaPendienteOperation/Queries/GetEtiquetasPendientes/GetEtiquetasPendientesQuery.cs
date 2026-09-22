using Domain.Dtos;
using MediatR;

namespace Application.UseCase.EtiquetaPendienteOperation.Queries.GetEtiquetasPendientes
{
    /// <summary>Toda la cola de etiquetas pendientes de imprimir, más antiguas primero (el orden
    /// en que se fueron agregando). De paso, descarta solas las filas de más de 30 días sin
    /// imprimirse — nadie tiene que acordarse de vaciar la cola a mano.</summary>
    public class GetEtiquetasPendientesQuery : IRequest<BaseResponse<List<EtiquetaPendienteDto>>>
    {
    }
}
