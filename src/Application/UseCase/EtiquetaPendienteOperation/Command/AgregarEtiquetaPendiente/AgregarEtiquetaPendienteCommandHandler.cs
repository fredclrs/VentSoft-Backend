using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.EtiquetaPendienteOperation.Command.AgregarEtiquetaPendiente
{
    /// <summary>
    /// Agrega un artículo a la cola de etiquetas pendientes de imprimir. Si ese artículo ya
    /// estaba en la cola, suma la cantidad a la fila existente en vez de duplicarla — mismo
    /// criterio que "agregar al carrito" en Ventas/Compras.
    /// </summary>
    public class AgregarEtiquetaPendienteCommandHandler : IRequestHandler<AgregarEtiquetaPendienteCommand, BaseResponse<EtiquetaPendienteDto>>
    {
        private readonly IRepository<EtiquetaPendiente> _repository;
        private readonly IArticuloRepository _articuloRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AgregarEtiquetaPendienteCommandHandler> _logger;

        public AgregarEtiquetaPendienteCommandHandler(
            IRepository<EtiquetaPendiente> repository,
            IArticuloRepository articuloRepository,
            IMapper mapper,
            ILogger<AgregarEtiquetaPendienteCommandHandler> logger)
        {
            _repository = repository;
            _articuloRepository = articuloRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<EtiquetaPendienteDto>> Handle(AgregarEtiquetaPendienteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dto = request.EtiquetaDto;

                var articulo = await _articuloRepository.GetByIdAsync(dto.IdArticulo);
                if (articulo == null)
                    return BaseResponse<EtiquetaPendienteDto>.FailureResponse("El artículo no existe.");

                var existente = (await _repository.GetAllAsync()).FirstOrDefault(e => e.IdArticulo == dto.IdArticulo);
                if (existente != null)
                {
                    existente.Cantidad += dto.Cantidad;
                    existente.FechaAgregado = DateTime.Now;
                    _repository.Update(existente);
                    await _repository.SaveChangesAsync();
                    return BaseResponse<EtiquetaPendienteDto>.SuccessResponse(_mapper.Map<EtiquetaPendienteDto>(existente), "Cantidad sumada a la cola de etiquetas.");
                }

                var nueva = new EtiquetaPendiente
                {
                    IdArticulo = dto.IdArticulo,
                    Cantidad = dto.Cantidad,
                    FechaAgregado = DateTime.Now,
                    UserAgregado = "system", // TODO: usuario autenticado real
                };

                await _repository.AddAsync(nueva);
                await _repository.SaveChangesAsync();

                return BaseResponse<EtiquetaPendienteDto>.SuccessResponse(_mapper.Map<EtiquetaPendienteDto>(nueva), "Agregado a la cola de etiquetas.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar a la cola de etiquetas");
                return BaseResponse<EtiquetaPendienteDto>.FailureResponse("Ocurrió un error al agregar a la cola de etiquetas.");
            }
        }
    }
}
