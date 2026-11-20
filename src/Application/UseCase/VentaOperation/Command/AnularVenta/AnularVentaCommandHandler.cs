using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.VentaOperation.Command.AnularVenta
{
    /// <summary>
    /// Anula una venta: Estado "AN" + motivo + quién/cuándo (UserBaja/FechaBaja). No borra nada.
    /// - Stock: vuelve solo, porque StockService no cuenta los renglones de ventas anuladas.
    /// - Deuda del cliente: deja de contar (cobros y cuentas por cobrar solo miran Estado "AC").
    /// - Saldo a favor que se usó para pagarla: se le devuelve al cliente.
    /// - Plata cobrada en el momento: sale de caja a mano; los reportes ya no la suman.
    ///
    /// Se frena si la venta tuvo movimientos posteriores (cobro, devolución o liquidación — los
    /// tres marcan FechaActualizado): esa plata ya quedó aplicada y deshacerla a ciegas
    /// descuadraría la cuenta del cliente. Para esos casos está Devolución.
    /// </summary>
    public class AnularVentaCommandHandler : IRequestHandler<AnularVentaCommand, BaseResponse<VentaDto>>
    {
        private readonly IVentaRepository _ventaRepository;
        private readonly IDevolucionVentaRepository _devolucionRepository;
        private readonly IRepository<Cliente> _clienteRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AnularVentaCommandHandler> _logger;

        public AnularVentaCommandHandler(
            IVentaRepository ventaRepository,
            IDevolucionVentaRepository devolucionRepository,
            IRepository<Cliente> clienteRepository,
            IMapper mapper,
            ILogger<AnularVentaCommandHandler> logger)
        {
            _ventaRepository = ventaRepository;
            _devolucionRepository = devolucionRepository;
            _clienteRepository = clienteRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<VentaDto>> Handle(AnularVentaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var venta = await _ventaRepository.GetByIdWithDetallesAsync(request.IdVenta);
                if (venta == null)
                    return BaseResponse<VentaDto>.FailureResponse("No se encontró la venta.");
                if (venta.Estado == "AN")
                    return BaseResponse<VentaDto>.FailureResponse($"La venta #{venta.Id} ya estaba anulada.");
                if (venta.Estado != "AC")
                    return BaseResponse<VentaDto>.FailureResponse($"La venta #{venta.Id} no está activa.");

                var devoluciones = await _devolucionRepository.GetByVentaAsync(venta.Id);
                if (devoluciones.Count > 0 || venta.FechaActualizado != null)
                    return BaseResponse<VentaDto>.FailureResponse(
                        $"La venta #{venta.Id} ya tuvo cobros o devoluciones después de registrarse, así que no se puede anular. " +
                        "Usá Devolución para deshacerla.");

                if (venta.MontoSaldoAFavorAplicado > 0)
                {
                    var cliente = await _clienteRepository.GetByIdAsync(venta.IdCliente);
                    if (cliente != null)
                    {
                        cliente.SaldoAFavor += venta.MontoSaldoAFavorAplicado;
                        cliente.UserActualizado = request.Usuario ?? "system";
                        cliente.FechaActualizado = DateTime.Now;
                        _clienteRepository.Update(cliente);
                    }
                }

                venta.Estado = "AN";
                venta.MotivoAnulacion = request.Motivo.Trim();
                venta.UserBaja = request.Usuario ?? "system";
                venta.FechaBaja = DateTime.Now;
                _ventaRepository.Update(venta);
                await _ventaRepository.SaveChangesAsync();

                return BaseResponse<VentaDto>.SuccessResponse(_mapper.Map<VentaDto>(venta), $"Venta #{venta.Id} anulada.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al anular la venta {Id}", request.IdVenta);
                return BaseResponse<VentaDto>.FailureResponse("Ocurrió un error al anular la venta.");
            }
        }
    }
}
