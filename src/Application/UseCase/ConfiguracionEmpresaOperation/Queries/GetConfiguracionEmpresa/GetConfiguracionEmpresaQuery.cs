using Domain.Dtos;
using MediatR;

namespace Application.UseCase.ConfiguracionEmpresaOperation.Queries.GetConfiguracionEmpresa
{
    /// <summary>No lleva Id: siempre devuelve la única fila de configuración (la crea si no existe).</summary>
    public class GetConfiguracionEmpresaQuery : IRequest<BaseResponse<ConfiguracionEmpresaDto>>
    {
    }
}
