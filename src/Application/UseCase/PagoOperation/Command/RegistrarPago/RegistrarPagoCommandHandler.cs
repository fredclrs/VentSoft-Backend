using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.PagoOperation.Command.RegistrarPago
{
    /// <summary>
    /// Registra un pago a un proveedor, aplicándolo (FIFO, por fecha) contra sus compras con saldo
    /// pendiente (Compra.PorPagar), y deja en Pago.DeudaActual el saldo total que se le debe al
    /// proveedor después de aplicar el pago.
    /// </summary>
    public class RegistrarPagoCommandHandler : IRequestHandler<RegistrarPagoCommand, BaseResponse<PagoDto>>
    {
        private readonly IRepository<Pago> _pagoRepository;
        private readonly ICompraRepository _compraRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RegistrarPagoCommandHandler> _logger;

        public RegistrarPagoCommandHandler(
            IRepository<Pago> pagoRepository,
            ICompraRepository compraRepository,
            IMapper mapper,
            ILogger<RegistrarPagoCommandHandler> logger)
        {
            _pagoRepository = pagoRepository;
            _compraRepository = compraRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<PagoDto>> Handle(RegistrarPagoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dto = request.PagoDto;

                var comprasPendientes = (await _compraRepository.GetByProveedorAsync(dto.IdProveedor))
                    .Where(c => c.Estado == "AC" && c.PorPagar > 0)
                    .OrderBy(c => c.Fecha)
                    .ToList();

                var deudaAntes = comprasPendientes.Sum(c => c.PorPagar);
                if (deudaAntes <= 0)
                    return BaseResponse<PagoDto>.FailureResponse("No hay deuda pendiente con este proveedor.");

                var montoAplicar = Math.Min(dto.Monto, deudaAntes);
                var restante = montoAplicar;

                foreach (var compra in comprasPendientes)
                {
                    if (restante <= 0) break;

                    var aplicado = Math.Min(compra.PorPagar, restante);
                    compra.Pagado += aplicado;
                    compra.PorPagar -= aplicado;
                    compra.UserActualizado = "system";
                    compra.FechaActualizado = DateTime.Now;
                    _compraRepository.Update(compra);

                    restante -= aplicado;
                }

                var pago = new Pago
                {
                    Fecha = dto.Fecha == default ? DateTime.Now : dto.Fecha,
                    Monto = dto.Monto,
                    Recibo = dto.Recibo,
                    Nota = dto.Nota,
                    IdProveedor = dto.IdProveedor,
                    IdUsuario = dto.IdUsuario,
                    DeudaActual = deudaAntes - montoAplicar,
                    Estado = "AC",
                    FechaRegistro = DateTime.Now,
                    UserRegistro = "system" // TODO: usuario autenticado real
                };

                await _pagoRepository.AddAsync(pago);
                await _pagoRepository.SaveChangesAsync();

                return BaseResponse<PagoDto>.SuccessResponse(_mapper.Map<PagoDto>(pago), "Pago registrado correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar el pago");
                return BaseResponse<PagoDto>.FailureResponse("Ocurrió un error al registrar el pago.");
            }
        }
    }
}
