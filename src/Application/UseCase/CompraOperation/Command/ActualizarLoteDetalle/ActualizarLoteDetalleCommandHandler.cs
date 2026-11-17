using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.CompraOperation.Command.ActualizarLoteDetalle
{
    public class ActualizarLoteDetalleCommandHandler : IRequestHandler<ActualizarLoteDetalleCommand, BaseResponse<DetalleCompraDto>>
    {
        private readonly ICompraRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ActualizarLoteDetalleCommandHandler> _logger;

        public ActualizarLoteDetalleCommandHandler(
            ICompraRepository repository,
            IMapper mapper,
            ILogger<ActualizarLoteDetalleCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<DetalleCompraDto>> Handle(ActualizarLoteDetalleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var detalle = await _repository.GetDetalleByIdAsync(request.IdDetalleCompra);
                if (detalle == null)
                    return BaseResponse<DetalleCompraDto>.FailureResponse("No se encontró ese renglón de compra.");

                // Solo estos dos campos: el resto del renglón (cantidad, costo, pagado) mueve stock
                // y plata, y no se corrige desde acá.
                detalle.Lote = string.IsNullOrWhiteSpace(request.Lote) ? null : request.Lote.Trim();
                detalle.FechaVencimiento = request.FechaVencimiento?.Date;
                await _repository.SaveChangesAsync();

                return BaseResponse<DetalleCompraDto>.SuccessResponse(
                    _mapper.Map<DetalleCompraDto>(detalle), "Vencimiento actualizado correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar lote/vencimiento del renglón de compra {Id}", request.IdDetalleCompra);
                return BaseResponse<DetalleCompraDto>.FailureResponse("Ocurrió un error al actualizar el vencimiento.");
            }
        }
    }
}
