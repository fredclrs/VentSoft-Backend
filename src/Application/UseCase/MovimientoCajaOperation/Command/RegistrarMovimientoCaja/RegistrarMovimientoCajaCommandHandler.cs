using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.MovimientoCajaOperation.Command.RegistrarMovimientoCaja
{
    /// <summary>
    /// Registra una entrada o salida de efectivo de la caja que no viene de una Venta ni de una
    /// Devolución/Cambio (por ejemplo, sacar plata para comprar algo puntual, o un gasto suelto).
    /// Se usa solo para que "Ventas del día" pueda calcular bien el efectivo esperado en caja.
    /// </summary>
    public class RegistrarMovimientoCajaCommandHandler : IRequestHandler<RegistrarMovimientoCajaCommand, BaseResponse<MovimientoCajaDto>>
    {
        private readonly IRepository<MovimientoCaja> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<RegistrarMovimientoCajaCommandHandler> _logger;

        public RegistrarMovimientoCajaCommandHandler(IRepository<MovimientoCaja> repository, IMapper mapper, ILogger<RegistrarMovimientoCajaCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<MovimientoCajaDto>> Handle(RegistrarMovimientoCajaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dto = request.MovimientoDto;

                var movimiento = new MovimientoCaja
                {
                    Fecha = dto.Fecha == default ? DateTime.Now : dto.Fecha,
                    Tipo = dto.Tipo,
                    Monto = dto.Monto,
                    Motivo = dto.Motivo,
                    IdUsuario = dto.IdUsuario,
                    Estado = "AC",
                    FechaRegistro = DateTime.Now,
                    UserRegistro = "system" // TODO: usuario autenticado real
                };

                await _repository.AddAsync(movimiento);
                await _repository.SaveChangesAsync();

                return BaseResponse<MovimientoCajaDto>.SuccessResponse(_mapper.Map<MovimientoCajaDto>(movimiento), "Movimiento de caja registrado correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar el movimiento de caja");
                return BaseResponse<MovimientoCajaDto>.FailureResponse("Ocurrió un error al registrar el movimiento de caja.");
            }
        }
    }
}
