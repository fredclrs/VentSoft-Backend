using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.VentaOperation.Command.RegistrarVenta
{
    /// <summary>
    /// Registra una Venta junto con todos sus renglones (DetalleVenta) en una sola operación,
    /// validando primero que haya stock suficiente de cada artículo antes de confirmar nada.
    /// Cada artículo involucrado se bloquea (ver IStockService.IniciarOperacionDeStockAsync)
    /// antes de leer su stock: si dos ventas del mismo artículo llegan casi juntas (dos
    /// cajeros vendiendo lo último que queda al mismo tiempo), la segunda espera a que la
    /// primera termine y recién ahí lee el stock ya actualizado — nunca pueden "pasar" el
    /// chequeo las dos a la vez y terminar vendiendo de más.
    ///
    /// De paso, se copia el costo actual de cada artículo a su DetalleVenta (CostoUnitario):
    /// así la ganancia de esta venta queda fija para siempre, sin importar que compras
    /// futuras cambien el costo promedio del artículo más adelante.
    /// </summary>
    public class RegistrarVentaCommandHandler : IRequestHandler<RegistrarVentaCommand, BaseResponse<VentaDto>>
    {
        private readonly IVentaRepository _ventaRepository;
        private readonly IArticuloRepository _articuloRepository;
        private readonly IRepository<Cliente> _clienteRepository;
        private readonly IStockService _stockService;
        private readonly IMapper _mapper;
        private readonly ILogger<RegistrarVentaCommandHandler> _logger;

        public RegistrarVentaCommandHandler(
            IVentaRepository ventaRepository,
            IArticuloRepository articuloRepository,
            IRepository<Cliente> clienteRepository,
            IStockService stockService,
            IMapper mapper,
            ILogger<RegistrarVentaCommandHandler> logger)
        {
            _ventaRepository = ventaRepository;
            _articuloRepository = articuloRepository;
            _clienteRepository = clienteRepository;
            _stockService = stockService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<VentaDto>> Handle(RegistrarVentaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dto = request.VentaDto;

                // 1) Validar stock disponible ANTES de tocar nada: se agrupa por artículo por si
                //    el mismo artículo aparece en más de un renglón de la misma venta.
                var cantidadPorArticulo = dto.Detalles
                    .GroupBy(d => d.IdArticulo)
                    .ToDictionary(g => g.Key, g => g.Sum(x => x.Cantidad));

                await using var stockTx = await _stockService.IniciarOperacionDeStockAsync();
                foreach (var idArticulo in cantidadPorArticulo.Keys)
                    await stockTx.BloquearArticuloAsync(idArticulo);

                // Costo de cada artículo AL MOMENTO de esta venta (se guarda en cada DetalleVenta
                // para que la ganancia de esta venta quede congelada para siempre, sin importar
                // que compras futuras cambien el costo promedio del artículo más adelante).
                var costoPorArticulo = new Dictionary<int, double>();

                foreach (var (idArticulo, cantidadTotal) in cantidadPorArticulo)
                {
                    var articulo = await _articuloRepository.GetByIdAsync(idArticulo);
                    if (articulo == null || articulo.Estado != "AC")
                        return BaseResponse<VentaDto>.FailureResponse($"El artículo con Id {idArticulo} no existe o está inactivo.");

                    var stockDisponible = await _stockService.ValidarStockAsync(idArticulo, cantidadTotal);
                    if (!stockDisponible)
                    {
                        var stockActual = await _stockService.GetStockActualAsync(idArticulo);
                        return BaseResponse<VentaDto>.FailureResponse(
                            $"Stock insuficiente para el artículo '{articulo.Codigo}' (disponible: {stockActual}, solicitado: {cantidadTotal}).");
                    }

                    // Articulo.Costo es "por caja completa" (igual que Precio), pero acá hace
                    // falta el costo POR UNIDAD real: DetalleVenta.Cantidad/PrecioUnitario ya
                    // vienen en unidades (el frontend convierte antes de mandar, sea la venta
                    // "por caja" o "por unidad suelta"). Sin dividir por Fraccion, la ganancia
                    // de artículos que se venden por caja saldría completamente mal.
                    var fraccion = articulo.Fraccion > 0 ? articulo.Fraccion : 1;
                    costoPorArticulo[idArticulo] = articulo.Costo / fraccion;
                }

                // 2) Stock validado para todos los renglones: recién ahora se arma y confirma la venta.
                var venta = new Venta
                {
                    Fecha = dto.Fecha == default ? DateTime.Now : dto.Fecha,
                    Referencias = dto.Referencias,
                    DescuentoMonetario = dto.DescuentoMonetario,
                    DescuentoPorcentaje = dto.DescuentoPorcentaje,
                    Nota = dto.Nota,
                    IdCliente = dto.IdCliente,
                    IdUsuario = dto.IdUsuario,
                    IdPromocion = dto.IdPromocion,
                    IdFormaDePago = dto.IdFormaDePago,
                    Estado = "AC",
                    FechaRegistro = DateTime.Now,
                    UserRegistro = "system" // TODO: usuario autenticado real
                };

                double subtotalGeneral = 0;

                foreach (var d in dto.Detalles)
                {
                    var bruto = d.PrecioUnitario * d.Cantidad;
                    var descuento = (d.DescuentoMonetario ?? 0) + (bruto * (d.DescuentoPorcentaje ?? 0) / 100.0);
                    var subTotal = Math.Max(0, bruto - descuento);
                    subtotalGeneral += subTotal;

                    venta.Detalles.Add(new DetalleVenta
                    {
                        IdArticulo = d.IdArticulo,
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.PrecioUnitario,
                        CostoUnitario = costoPorArticulo.GetValueOrDefault(d.IdArticulo),
                        DescuentoMonetario = d.DescuentoMonetario,
                        DescuentoPorcentaje = d.DescuentoPorcentaje,
                        SubTotal = subTotal,
                        Pagado = 0
                    });
                }

                // Descuento adicional a nivel de venta completa (encima de los descuentos por renglón).
                var descuentoGeneral = (dto.DescuentoMonetario ?? 0) + (subtotalGeneral * (dto.DescuentoPorcentaje ?? 0) / 100.0);
                var total = Math.Max(0, subtotalGeneral - descuentoGeneral);

                venta.Total = total;
                venta.Pagado = dto.Pagado;
                venta.PorPagar = total - dto.Pagado;

                // Saldo a favor del cliente (por una devolución/cambio anterior): opcional, se aplica
                // como un pago más, tope al saldo disponible y a lo que todavía falta pagar.
                if (dto.MontoSaldoAFavorAplicado > 0)
                {
                    var cliente = await _clienteRepository.GetByIdAsync(dto.IdCliente);
                    if (cliente == null)
                        return BaseResponse<VentaDto>.FailureResponse("Cliente no encontrado.");

                    var montoAplicado = Math.Clamp(dto.MontoSaldoAFavorAplicado, 0, Math.Min(cliente.SaldoAFavor, venta.PorPagar));
                    if (montoAplicado > 0)
                    {
                        cliente.SaldoAFavor -= montoAplicado;
                        cliente.UserActualizado = "system";
                        cliente.FechaActualizado = DateTime.Now;
                        _clienteRepository.Update(cliente);

                        venta.MontoSaldoAFavorAplicado = montoAplicado;
                        venta.Pagado += montoAplicado;
                        venta.PorPagar -= montoAplicado;
                    }
                }

                await _ventaRepository.AddAsync(venta);
                await _ventaRepository.SaveChangesAsync();
                await stockTx.ConfirmarAsync();

                var creada = await _ventaRepository.GetByIdWithDetallesAsync(venta.Id);
                return BaseResponse<VentaDto>.SuccessResponse(_mapper.Map<VentaDto>(creada), "Venta registrada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar la venta");
                return BaseResponse<VentaDto>.FailureResponse("Ocurrió un error al registrar la venta.");
            }
        }
    }
}
