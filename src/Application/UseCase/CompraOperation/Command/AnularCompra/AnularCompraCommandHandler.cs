using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.CompraOperation.Command.AnularCompra
{
    /// <summary>
    /// Anula una compra: Estado "AN" + motivo + quién/cuándo (UserBaja/FechaBaja). No borra nada.
    /// - Stock: su mercadería deja de contar (StockService ignora renglones de compras anuladas).
    /// - Deuda con el proveedor: deja de contar (pagos y cuentas por pagar solo miran "AC").
    /// - Lotes por vencer: sus lotes dejan de aparecer.
    /// - Costo del artículo: NO se recalcula (el costo promedio ya se mezcló con compras
    ///   posteriores); si quedó mal, se corrige a mano en Artículos.
    ///
    /// Frenos: no se anula si tuvo pagos posteriores (RegistrarPago marca FechaActualizado) ni si
    /// sacar su mercadería deja algún artículo con stock negativo (ya se vendió parte) — para eso
    /// está Ajuste de stock. Los artículos se bloquean antes de leer el stock, igual que en
    /// Venta, para que una venta simultánea no se cuele entre el chequeo y la anulación.
    /// </summary>
    public class AnularCompraCommandHandler : IRequestHandler<AnularCompraCommand, BaseResponse<CompraDto>>
    {
        private readonly ICompraRepository _compraRepository;
        private readonly IStockService _stockService;
        private readonly IMapper _mapper;
        private readonly ILogger<AnularCompraCommandHandler> _logger;

        public AnularCompraCommandHandler(
            ICompraRepository compraRepository,
            IStockService stockService,
            IMapper mapper,
            ILogger<AnularCompraCommandHandler> logger)
        {
            _compraRepository = compraRepository;
            _stockService = stockService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<CompraDto>> Handle(AnularCompraCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var compra = await _compraRepository.GetByIdWithDetallesAsync(request.IdCompra);
                if (compra == null)
                    return BaseResponse<CompraDto>.FailureResponse("No se encontró la compra.");
                if (compra.Estado == "AN")
                    return BaseResponse<CompraDto>.FailureResponse($"La compra #{compra.Id} ya estaba anulada.");
                if (compra.Estado != "AC")
                    return BaseResponse<CompraDto>.FailureResponse($"La compra #{compra.Id} no está activa.");
                if (compra.FechaActualizado != null)
                    return BaseResponse<CompraDto>.FailureResponse(
                        $"La compra #{compra.Id} ya tuvo pagos al proveedor después de registrarse, así que no se puede anular.");

                var cantidadPorArticulo = compra.Detalles
                    .GroupBy(d => d.IdArticulo)
                    .ToDictionary(g => g.Key, g => g.Sum(x => x.Cantidad));

                await using var stockTx = await _stockService.IniciarOperacionDeStockAsync();
                foreach (var idArticulo in cantidadPorArticulo.Keys)
                    await stockTx.BloquearArticuloAsync(idArticulo);

                foreach (var (idArticulo, cantidad) in cantidadPorArticulo)
                {
                    var stockActual = await _stockService.GetStockActualAsync(idArticulo);
                    if (stockActual - cantidad < 0)
                    {
                        var articulo = compra.Detalles.First(d => d.IdArticulo == idArticulo).Articulo;
                        return BaseResponse<CompraDto>.FailureResponse(
                            $"No se puede anular: de '{articulo?.Descripcion ?? articulo?.Codigo}' entraron {cantidad} con esta compra " +
                            $"pero quedan {stockActual} en stock (ya se vendió parte). Corregilo con Ajuste de stock.");
                    }
                }

                compra.Estado = "AN";
                compra.MotivoAnulacion = request.Motivo.Trim();
                compra.UserBaja = request.Usuario ?? "system";
                compra.FechaBaja = DateTime.Now;
                _compraRepository.Update(compra);
                await _compraRepository.SaveChangesAsync();
                await stockTx.ConfirmarAsync();

                return BaseResponse<CompraDto>.SuccessResponse(_mapper.Map<CompraDto>(compra), $"Compra #{compra.Id} anulada.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al anular la compra {Id}", request.IdCompra);
                return BaseResponse<CompraDto>.FailureResponse("Ocurrió un error al anular la compra.");
            }
        }
    }
}
