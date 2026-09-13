using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.AjusteStockOperation.Command.RegistrarAjusteStock
{
    /// <summary>
    /// Registra una corrección manual de stock (rotura, vencimiento, robo, conteo real distinto)
    /// que no viene de una Compra ni de una Venta. En una SALIDA, bloquea el artículo (mismo
    /// mecanismo que Venta/Cambio) y valida que haya stock suficiente antes de confirmar — no se
    /// puede ajustar stock a un número negativo.
    /// </summary>
    public class RegistrarAjusteStockCommandHandler : IRequestHandler<RegistrarAjusteStockCommand, BaseResponse<AjusteStockDto>>
    {
        private readonly IRepository<AjusteStock> _repository;
        private readonly IArticuloRepository _articuloRepository;
        private readonly IStockService _stockService;
        private readonly IMapper _mapper;
        private readonly ILogger<RegistrarAjusteStockCommandHandler> _logger;

        public RegistrarAjusteStockCommandHandler(
            IRepository<AjusteStock> repository,
            IArticuloRepository articuloRepository,
            IStockService stockService,
            IMapper mapper,
            ILogger<RegistrarAjusteStockCommandHandler> logger)
        {
            _repository = repository;
            _articuloRepository = articuloRepository;
            _stockService = stockService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<AjusteStockDto>> Handle(RegistrarAjusteStockCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dto = request.AjusteDto;

                var articulo = await _articuloRepository.GetByIdAsync(dto.IdArticulo);
                if (articulo == null || articulo.Estado != "AC")
                    return BaseResponse<AjusteStockDto>.FailureResponse("El artículo no existe o está inactivo.");

                await using var stockTx = await _stockService.IniciarOperacionDeStockAsync();

                if (dto.Tipo == "SALIDA")
                {
                    await stockTx.BloquearArticuloAsync(dto.IdArticulo);

                    var stockDisponible = await _stockService.ValidarStockAsync(dto.IdArticulo, dto.Cantidad);
                    if (!stockDisponible)
                    {
                        var stockActual = await _stockService.GetStockActualAsync(dto.IdArticulo);
                        return BaseResponse<AjusteStockDto>.FailureResponse(
                            $"No se puede ajustar: el stock disponible ({stockActual}) es menor a la cantidad a dar de baja ({dto.Cantidad}).");
                    }
                }

                var ajuste = new AjusteStock
                {
                    Fecha = dto.Fecha == default ? DateTime.Now : dto.Fecha,
                    Tipo = dto.Tipo,
                    Cantidad = dto.Cantidad,
                    Motivo = dto.Motivo,
                    IdArticulo = dto.IdArticulo,
                    IdUsuario = dto.IdUsuario,
                    Estado = "AC",
                    FechaRegistro = DateTime.Now,
                    UserRegistro = "system" // TODO: usuario autenticado real
                };

                await _repository.AddAsync(ajuste);
                await _repository.SaveChangesAsync();
                await stockTx.ConfirmarAsync();

                return BaseResponse<AjusteStockDto>.SuccessResponse(_mapper.Map<AjusteStockDto>(ajuste), "Ajuste de stock registrado correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar el ajuste de stock");
                return BaseResponse<AjusteStockDto>.FailureResponse("Ocurrió un error al registrar el ajuste de stock.");
            }
        }
    }
}
