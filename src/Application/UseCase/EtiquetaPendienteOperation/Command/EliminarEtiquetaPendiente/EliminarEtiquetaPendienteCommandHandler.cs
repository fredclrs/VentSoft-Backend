using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.EtiquetaPendienteOperation.Command.EliminarEtiquetaPendiente
{
    /// <summary>Saca una fila de la cola de etiquetas — borrado físico (no es baja lógica, esto
    /// no es un registro contable, es solo la lista de lo que falta imprimir).</summary>
    public class EliminarEtiquetaPendienteCommandHandler : IRequestHandler<EliminarEtiquetaPendienteCommand, BaseResponse<bool>>
    {
        private readonly IRepository<EtiquetaPendiente> _repository;
        private readonly ILogger<EliminarEtiquetaPendienteCommandHandler> _logger;

        public EliminarEtiquetaPendienteCommandHandler(IRepository<EtiquetaPendiente> repository, ILogger<EliminarEtiquetaPendienteCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<BaseResponse<bool>> Handle(EliminarEtiquetaPendienteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var etiqueta = await _repository.GetByIdAsync(request.Id);
                if (etiqueta == null)
                    return BaseResponse<bool>.FailureResponse("No se encontró esa fila en la cola de etiquetas.");

                _repository.Delete(etiqueta);
                await _repository.SaveChangesAsync();

                return BaseResponse<bool>.SuccessResponse(true, "Sacado de la cola de etiquetas.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al sacar una fila de la cola de etiquetas");
                return BaseResponse<bool>.FailureResponse("Ocurrió un error al sacarlo de la cola de etiquetas.");
            }
        }
    }
}
