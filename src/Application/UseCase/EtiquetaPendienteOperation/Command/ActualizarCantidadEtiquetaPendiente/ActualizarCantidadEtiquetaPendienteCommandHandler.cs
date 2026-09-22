using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.EtiquetaPendienteOperation.Command.ActualizarCantidadEtiquetaPendiente
{
    /// <summary>Corrige la cantidad de una fila ya cargada en la cola, sin tener que sacarla y
    /// volver a agregarla (ej. la vendedora se equivocó al tipear la cantidad).</summary>
    public class ActualizarCantidadEtiquetaPendienteCommandHandler : IRequestHandler<ActualizarCantidadEtiquetaPendienteCommand, BaseResponse<EtiquetaPendienteDto>>
    {
        private readonly IRepository<EtiquetaPendiente> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ActualizarCantidadEtiquetaPendienteCommandHandler> _logger;

        public ActualizarCantidadEtiquetaPendienteCommandHandler(
            IRepository<EtiquetaPendiente> repository,
            IMapper mapper,
            ILogger<ActualizarCantidadEtiquetaPendienteCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<EtiquetaPendienteDto>> Handle(ActualizarCantidadEtiquetaPendienteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Cantidad <= 0)
                    return BaseResponse<EtiquetaPendienteDto>.FailureResponse("La cantidad debe ser mayor a 0.");

                var etiqueta = await _repository.GetByIdAsync(request.Id);
                if (etiqueta == null)
                    return BaseResponse<EtiquetaPendienteDto>.FailureResponse("No se encontró esa fila en la cola de etiquetas.");

                etiqueta.Cantidad = request.Cantidad;
                _repository.Update(etiqueta);
                await _repository.SaveChangesAsync();

                return BaseResponse<EtiquetaPendienteDto>.SuccessResponse(_mapper.Map<EtiquetaPendienteDto>(etiqueta), "Cantidad actualizada.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la cantidad en la cola de etiquetas");
                return BaseResponse<EtiquetaPendienteDto>.FailureResponse("Ocurrió un error al actualizar la cantidad.");
            }
        }
    }
}
