using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.DevolucionOperation.Command.RegistrarDevolucion
{
    /// <summary>
    /// Registra una devolución (o un cambio, si trae ArticulosCambio) de artículos de una Venta
    /// existente. Siempre referencia esa venta: no se admiten devoluciones sin venta asociada.
    ///
    /// Flujo de dinero (ver comentario en Domain.Entities.DevolucionVenta): el Total de la venta
    /// se recalcula sacando lo devuelto y sumando lo nuevo entregado (si es un cambio) EN UN SOLO
    /// paso — no por separado — porque el saldo pendiente que ya tenía la venta por el artículo
    /// devuelto sigue siendo válido para cubrir el artículo nuevo (es la misma deuda continuando,
    /// no una deuda que se perdona para después cobrar el artículo nuevo desde cero). Sobre ese
    /// nuevo Total:
    /// 1) Si el cliente ya había pagado de más (pagó por algo que ahora se llevó de vuelta y no
    ///    cambió por nada de igual valor), sobra plata: efectivo ahora o saldo a favor, a elección
    ///    del cajero.
    /// 2) Si la nueva deuda de la venta es mayor a la que tenía antes de esta operación (se llevó
    ///    algo de más valor que lo devuelto), esa diferencia extra se cobra ahora y/o queda
    ///    pendiente — nunca el valor completo del artículo nuevo, solo lo que exceda a lo que ya
    ///    se debía.
    ///
    /// Los artículos del cambio se bloquean (ver IStockService.IniciarOperacionDeStockAsync)
    /// antes de leer su stock, para que no puedan "chocar" con una Venta u otro Cambio
    /// concurrente del mismo artículo.
    /// </summary>
    public class RegistrarDevolucionCommandHandler : IRequestHandler<RegistrarDevolucionCommand, BaseResponse<DevolucionVentaDto>>
    {
        private readonly IDevolucionVentaRepository _devolucionRepository;
        private readonly IVentaRepository _ventaRepository;
        private readonly IArticuloRepository _articuloRepository;
        private readonly IRepository<Cliente> _clienteRepository;
        private readonly IStockService _stockService;
        private readonly IMapper _mapper;
        private readonly ILogger<RegistrarDevolucionCommandHandler> _logger;

        public RegistrarDevolucionCommandHandler(
            IDevolucionVentaRepository devolucionRepository,
            IVentaRepository ventaRepository,
            IArticuloRepository articuloRepository,
            IRepository<Cliente> clienteRepository,
            IStockService stockService,
            IMapper mapper,
            ILogger<RegistrarDevolucionCommandHandler> logger)
        {
            _devolucionRepository = devolucionRepository;
            _ventaRepository = ventaRepository;
            _articuloRepository = articuloRepository;
            _clienteRepository = clienteRepository;
            _stockService = stockService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<DevolucionVentaDto>> Handle(RegistrarDevolucionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dto = request.DevolucionDto;

                var venta = await _ventaRepository.GetByIdWithDetallesAsync(dto.IdVenta);
                if (venta == null || venta.Estado != "AC")
                    return BaseResponse<DevolucionVentaDto>.FailureResponse("La venta original no existe o está inactiva.");

                // Cuánto de cada renglón original ya se devolvió antes, para no exceder lo vendido.
                var devolucionesPrevias = await _devolucionRepository.GetByVentaAsync(dto.IdVenta);
                var devueltoPorLinea = devolucionesPrevias
                    .SelectMany(d => d.Detalles)
                    .GroupBy(d => d.IdDetalleVenta)
                    .ToDictionary(g => g.Key, g => g.Sum(x => x.Cantidad));

                var detallesVentaPorId = venta.Detalles.ToDictionary(d => d.Id);

                double totalDevuelto = 0;
                var detallesDevueltos = new List<DetalleDevolucionVenta>();

                foreach (var d in dto.Detalles)
                {
                    if (!detallesVentaPorId.TryGetValue(d.IdDetalleVenta, out var detalleOriginal))
                        return BaseResponse<DevolucionVentaDto>.FailureResponse($"El renglón {d.IdDetalleVenta} no pertenece a esta venta.");

                    var yaDevuelto = devueltoPorLinea.GetValueOrDefault(d.IdDetalleVenta);
                    var disponibleParaDevolver = detalleOriginal.Cantidad - yaDevuelto;
                    if (d.Cantidad > disponibleParaDevolver)
                        return BaseResponse<DevolucionVentaDto>.FailureResponse(
                            $"No se puede devolver {d.Cantidad} unidad(es) del artículo '{detalleOriginal.Articulo?.Codigo}' — solo quedan {disponibleParaDevolver} disponibles para devolver de esa venta.");

                    var subTotal = detalleOriginal.PrecioUnitario * d.Cantidad;
                    totalDevuelto += subTotal;

                    detallesDevueltos.Add(new DetalleDevolucionVenta
                    {
                        IdDetalleVenta = d.IdDetalleVenta,
                        IdArticulo = detalleOriginal.IdArticulo,
                        Cantidad = d.Cantidad,
                        PrecioUnitario = detalleOriginal.PrecioUnitario,
                        SubTotal = subTotal,
                        Vendible = d.Vendible,
                    });
                }

                // Artículos nuevos entregados a cambio (vacío si es una devolución pura). Se
                // bloquean antes de leer su stock: si una Venta o Cambio concurrente del mismo
                // artículo está en curso, esta operación espera a que termine — así nunca dos
                // operaciones pueden "pasar" el chequeo de stock al mismo tiempo.
                double totalCambio = 0;
                var articulosCambio = new List<DetalleCambioVenta>();

                await using var stockTx = await _stockService.IniciarOperacionDeStockAsync();
                foreach (var idArticuloCambio in dto.ArticulosCambio.Select(a => a.IdArticulo).Distinct())
                    await stockTx.BloquearArticuloAsync(idArticuloCambio);

                foreach (var a in dto.ArticulosCambio)
                {
                    var articulo = await _articuloRepository.GetByIdAsync(a.IdArticulo);
                    if (articulo == null || articulo.Estado != "AC")
                        return BaseResponse<DevolucionVentaDto>.FailureResponse($"El artículo con Id {a.IdArticulo} no existe o está inactivo.");

                    var stockDisponible = await _stockService.ValidarStockAsync(a.IdArticulo, a.Cantidad);
                    if (!stockDisponible)
                    {
                        var stockActual = await _stockService.GetStockActualAsync(a.IdArticulo);
                        return BaseResponse<DevolucionVentaDto>.FailureResponse(
                            $"Stock insuficiente para el artículo '{articulo.Codigo}' (disponible: {stockActual}, solicitado: {a.Cantidad}).");
                    }

                    var subTotal = a.PrecioUnitario * a.Cantidad;
                    totalCambio += subTotal;

                    articulosCambio.Add(new DetalleCambioVenta
                    {
                        IdArticulo = a.IdArticulo,
                        Cantidad = a.Cantidad,
                        PrecioUnitario = a.PrecioUnitario,
                        SubTotal = subTotal,
                    });
                }

                var porPagarAntes = venta.PorPagar;

                // Se recalcula el Total sacando lo devuelto y sumando lo nuevo (si es un cambio) EN
                // UN SOLO paso: la deuda que ya tenía la venta por el artículo devuelto no se
                // "perdona" para volver a cobrar el nuevo desde cero, sigue siendo la misma deuda.
                venta.Total = Math.Max(0, venta.Total - totalDevuelto + totalCambio);
                var porPagarBruto = venta.Total - venta.Pagado;
                var sobrante = porPagarBruto < 0 ? -porPagarBruto : 0;
                var nuevaDeuda = porPagarBruto < 0 ? 0 : porPagarBruto;
                venta.PorPagar = nuevaDeuda;

                // Solo informativo (para el registro/impresión): cuánto del valor devuelto sirvió
                // para cancelar deuda que la venta ya tenía pendiente.
                var aplicadoADeuda = Math.Min(porPagarAntes, totalDevuelto);

                venta.UserActualizado = "system";
                venta.FechaActualizado = DateTime.Now;

                // Cuánta deuda NUEVA generó esta operación (por encima de la que la venta ya tenía) —
                // nunca el valor completo del artículo nuevo, solo el excedente real.
                var extraAPagar = Math.Max(0, nuevaDeuda - porPagarAntes);

                double montoCobradoAhora = 0, porPagarDevolucion = 0, montoDevueltoEfectivo = 0, saldoAFavorGenerado = 0;

                if (extraAPagar > 0)
                {
                    montoCobradoAhora = Math.Clamp(dto.MontoCobradoAhora, 0, extraAPagar);
                    if (montoCobradoAhora > 0)
                    {
                        venta.Pagado += montoCobradoAhora;
                        venta.PorPagar -= montoCobradoAhora;
                    }
                    porPagarDevolucion = extraAPagar - montoCobradoAhora;
                }
                else if (sobrante > 0)
                {
                    var cliente = await _clienteRepository.GetByIdAsync(venta.IdCliente);
                    if (cliente == null)
                        return BaseResponse<DevolucionVentaDto>.FailureResponse("Cliente no encontrado.");

                    if (dto.DevolverEnEfectivo)
                    {
                        montoDevueltoEfectivo = sobrante;
                    }
                    else
                    {
                        saldoAFavorGenerado = sobrante;
                        cliente.SaldoAFavor += sobrante;
                        cliente.UserActualizado = "system";
                        cliente.FechaActualizado = DateTime.Now;
                        _clienteRepository.Update(cliente);
                    }
                }

                var devolucion = new DevolucionVenta
                {
                    Fecha = DateTime.Now,
                    Motivo = dto.Motivo,
                    IdVenta = venta.Id,
                    IdCliente = venta.IdCliente,
                    IdUsuario = dto.IdUsuario,
                    TotalDevuelto = totalDevuelto,
                    TotalCambio = totalCambio,
                    AplicadoADeudaVenta = aplicadoADeuda,
                    MontoCobradoAhora = montoCobradoAhora,
                    PorPagar = porPagarDevolucion,
                    MontoDevueltoEfectivo = montoDevueltoEfectivo,
                    SaldoAFavorGenerado = saldoAFavorGenerado,
                    Estado = "AC",
                    FechaRegistro = DateTime.Now,
                    UserRegistro = "system", // TODO: usuario autenticado real
                    Detalles = detallesDevueltos,
                    ArticulosCambio = articulosCambio,
                };

                _ventaRepository.Update(venta);
                await _devolucionRepository.AddAsync(devolucion);
                await _devolucionRepository.SaveChangesAsync();
                await stockTx.ConfirmarAsync();

                var creada = await _devolucionRepository.GetByIdWithDetallesAsync(devolucion.Id);
                return BaseResponse<DevolucionVentaDto>.SuccessResponse(_mapper.Map<DevolucionVentaDto>(creada), "Devolución registrada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar la devolución");
                return BaseResponse<DevolucionVentaDto>.FailureResponse("Ocurrió un error al registrar la devolución.");
            }
        }
    }
}
