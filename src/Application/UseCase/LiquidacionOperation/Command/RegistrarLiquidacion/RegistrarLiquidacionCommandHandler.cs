using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.LiquidacionOperation.Command.RegistrarLiquidacion
{
    /// <summary>
    /// Liquida una o más boletas (EntregaBien) pendientes de un cliente: les fija el precio
    /// definitivo y aplica el valor total contra su deuda general (todas sus Ventas con saldo
    /// pendiente, más antiguas primero — exactamente igual que un Cobro en efectivo, ver
    /// RegistrarCobroCommandHandler). Si el valor entregado supera la deuda, el sobrante se
    /// devuelve en efectivo o se acredita como saldo a favor (Cliente.SaldoAFavor), a elección
    /// de quien liquida.
    /// </summary>
    public class RegistrarLiquidacionCommandHandler : IRequestHandler<RegistrarLiquidacionCommand, BaseResponse<LiquidacionDto>>
    {
        private readonly ILiquidacionRepository _liquidacionRepository;
        private readonly IEntregaBienRepository _entregaRepository;
        private readonly IVentaRepository _ventaRepository;
        private readonly IRepository<Cliente> _clienteRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RegistrarLiquidacionCommandHandler> _logger;

        public RegistrarLiquidacionCommandHandler(
            ILiquidacionRepository liquidacionRepository,
            IEntregaBienRepository entregaRepository,
            IVentaRepository ventaRepository,
            IRepository<Cliente> clienteRepository,
            IMapper mapper,
            ILogger<RegistrarLiquidacionCommandHandler> logger)
        {
            _liquidacionRepository = liquidacionRepository;
            _entregaRepository = entregaRepository;
            _ventaRepository = ventaRepository;
            _clienteRepository = clienteRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<LiquidacionDto>> Handle(RegistrarLiquidacionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dto = request.LiquidacionDto;

                // 1) Cargar y fijar precio definitivo de cada boleta incluida.
                var boletas = new List<EntregaBien>();
                double totalEntregado = 0;

                foreach (var e in dto.Entregas)
                {
                    var boleta = await _entregaRepository.GetByIdAsync(e.IdEntrega);
                    if (boleta == null || boleta.Estado != "AC")
                        return BaseResponse<LiquidacionDto>.FailureResponse($"La entrega {e.IdEntrega} no existe o está inactiva.");
                    if (boleta.IdCliente != dto.IdCliente)
                        return BaseResponse<LiquidacionDto>.FailureResponse($"La entrega {e.IdEntrega} no pertenece a este cliente.");
                    if (boleta.IdLiquidacion != null)
                        return BaseResponse<LiquidacionDto>.FailureResponse($"La entrega {e.IdEntrega} ya fue liquidada anteriormente.");

                    boleta.PrecioUnitario = e.PrecioUnitario;
                    boleta.SubTotal = boleta.Cantidad * e.PrecioUnitario;
                    boleta.UserActualizado = "system";
                    boleta.FechaActualizado = DateTime.Now;
                    _entregaRepository.Update(boleta);

                    totalEntregado += boleta.SubTotal.Value;
                    boletas.Add(boleta);
                }

                // 2) Aplicar contra la deuda general del cliente (FIFO por fecha, igual que un Cobro).
                var ventasPendientes = (await _ventaRepository.GetByClienteAsync(dto.IdCliente))
                    .Where(v => v.Estado == "AC" && v.PorPagar > 0)
                    .OrderBy(v => v.Fecha)
                    .ToList();

                var deudaAntes = ventasPendientes.Sum(v => v.PorPagar);
                var montoAplicar = Math.Min(totalEntregado, deudaAntes);
                var restante = montoAplicar;

                foreach (var venta in ventasPendientes)
                {
                    if (restante <= 0) break;

                    var aplicado = Math.Min(venta.PorPagar, restante);
                    venta.Pagado += aplicado;
                    venta.PorPagar -= aplicado;
                    venta.UserActualizado = "system";
                    venta.FechaActualizado = DateTime.Now;
                    _ventaRepository.Update(venta);

                    restante -= aplicado;
                }

                // 3) Lo que sobra del valor entregado (si alcanzó y sobró) es a favor del cliente.
                var sobrante = totalEntregado - montoAplicar;
                double montoDevueltoEfectivo = 0, saldoAFavorGenerado = 0;

                if (sobrante > 0)
                {
                    var cliente = await _clienteRepository.GetByIdAsync(dto.IdCliente);
                    if (cliente == null)
                        return BaseResponse<LiquidacionDto>.FailureResponse("Cliente no encontrado.");

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

                var liquidacion = new Liquidacion
                {
                    Fecha = DateTime.Now,
                    Nota = dto.Nota,
                    IdCliente = dto.IdCliente,
                    IdUsuario = dto.IdUsuario,
                    DeudaAntes = deudaAntes,
                    TotalEntregado = totalEntregado,
                    DeudaActual = deudaAntes - montoAplicar,
                    MontoDevueltoEfectivo = montoDevueltoEfectivo,
                    SaldoAFavorGenerado = saldoAFavorGenerado,
                    Estado = "AC",
                    FechaRegistro = DateTime.Now,
                    UserRegistro = "system", // TODO: usuario autenticado real
                };

                // Reparenta las boletas ya cargadas a esta liquidación nueva; EF resuelve el FK
                // (IdLiquidacion) solo al guardar, en el mismo SaveChanges.
                foreach (var boleta in boletas)
                    liquidacion.Entregas.Add(boleta);

                await _liquidacionRepository.AddAsync(liquidacion);
                await _liquidacionRepository.SaveChangesAsync();

                var creada = await _liquidacionRepository.GetByIdWithDetallesAsync(liquidacion.Id);
                return BaseResponse<LiquidacionDto>.SuccessResponse(_mapper.Map<LiquidacionDto>(creada), "Liquidación registrada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar la liquidación");
                return BaseResponse<LiquidacionDto>.FailureResponse("Ocurrió un error al registrar la liquidación.");
            }
        }
    }
}
