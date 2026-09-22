using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.EtiquetaPendienteOperation.Command.VaciarEtiquetasPendientes
{
    public class VaciarEtiquetasPendientesCommandHandler : IRequestHandler<VaciarEtiquetasPendientesCommand, BaseResponse<int>>
    {
        private readonly IRepository<EtiquetaPendiente> _repository;
        private readonly ILogger<VaciarEtiquetasPendientesCommandHandler> _logger;

        public VaciarEtiquetasPendientesCommandHandler(IRepository<EtiquetaPendiente> repository, ILogger<VaciarEtiquetasPendientesCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<BaseResponse<int>> Handle(VaciarEtiquetasPendientesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var todas = await _repository.GetAllAsync();
                foreach (var etiqueta in todas)
                {
                    _repository.Delete(etiqueta);
                }
                await _repository.SaveChangesAsync();

                return BaseResponse<int>.SuccessResponse(todas.Count, "Cola de etiquetas vaciada.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al vaciar la cola de etiquetas");
                return BaseResponse<int>.FailureResponse("Ocurrió un error al vaciar la cola de etiquetas.");
            }
        }
    }
}
