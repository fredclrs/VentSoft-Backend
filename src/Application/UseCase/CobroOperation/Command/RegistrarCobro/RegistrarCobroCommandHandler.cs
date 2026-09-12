using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.CobroOperation.Command.RegistrarCobro
{
    /// <summary>
    /// Registra un cobro a un cliente, aplicándolo (FIFO, por fecha) contra sus ventas con saldo
    /// pendiente (Venta.PorPagar), y deja en Cobro.DeudaActual el saldo total que le queda al cliente
    /// después de aplicar el pago.
    /// </summary>
    public class RegistrarCobroCommandHandler : IRequestHandler<RegistrarCobroCommand, BaseResponse<CobroDto>>
    {
        private readonly IRepository<Cobro> _cobroRepository;
        private readonly IVentaRepository _ventaRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RegistrarCobroCommandHandler> _logger;

        public RegistrarCobroCommandHandler(
            IRepository<Cobro> cobroRepository,
            IVentaRepository ventaRepository,
            IMapper mapper,
            ILogger<RegistrarCobroCommandHandler> logger)
        {
            _cobroRepository = cobroRepository;
            _ventaRepository = ventaRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<CobroDto>> Handle(RegistrarCobroCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dto = request.CobroDto;

                var ventasPendientes = (await _ventaRepository.GetByClienteAsync(dto.IdCliente))
                    .Where(v => v.Estado == "AC" && v.PorPagar > 0)
                    .OrderBy(v => v.Fecha)
                    .ToList();

                var deudaAntes = ventasPendientes.Sum(v => v.PorPagar);
                if (deudaAntes <= 0)
                    return BaseResponse<CobroDto>.FailureResponse("El cliente no tiene deuda pendiente.");

                var montoAplicar = Math.Min(dto.Monto, deudaAntes);
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

                var cobro = new Cobro
                {
                    Fecha = dto.Fecha == default ? DateTime.Now : dto.Fecha,
                    Monto = dto.Monto,
                    Recibo = dto.Recibo,
                    Nota = dto.Nota,
                    IdCliente = dto.IdCliente,
                    IdUsuario = dto.IdUsuario,
                    DeudaActual = deudaAntes - montoAplicar,
                    Estado = "AC",
                    FechaRegistro = DateTime.Now,
                    UserRegistro = "system" // TODO: usuario autenticado real
                };

                await _cobroRepository.AddAsync(cobro);
                // Una sola llamada a SaveChangesAsync: persiste el Cobro y las Ventas actualizadas juntos.
                await _cobroRepository.SaveChangesAsync();

                return BaseResponse<CobroDto>.SuccessResponse(_mapper.Map<CobroDto>(cobro), "Cobro registrado correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar el cobro");
                return BaseResponse<CobroDto>.FailureResponse("Ocurrió un error al registrar el cobro.");
            }
        }
    }
}
