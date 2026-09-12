using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.EntregaBienOperation.Command.RegistrarEntregaBien
{
    /// <summary>
    /// Registra una boleta de entrega de un bien (ej: grano) que el cliente trae para pagar su
    /// deuda en especie. No afecta la deuda todavía: eso pasa recién al liquidarla (ver
    /// RegistrarLiquidacionCommandHandler), momento en el que se fija el precio definitivo.
    /// </summary>
    public class RegistrarEntregaBienCommandHandler : IRequestHandler<RegistrarEntregaBienCommand, BaseResponse<EntregaBienDto>>
    {
        private readonly IEntregaBienRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<RegistrarEntregaBienCommandHandler> _logger;

        public RegistrarEntregaBienCommandHandler(IEntregaBienRepository repository, IMapper mapper, ILogger<RegistrarEntregaBienCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<EntregaBienDto>> Handle(RegistrarEntregaBienCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dto = request.EntregaDto;

                var entrega = new EntregaBien
                {
                    Fecha = dto.Fecha == default ? DateTime.Now : dto.Fecha,
                    NumeroBoleta = dto.NumeroBoleta,
                    Cantidad = dto.Cantidad,
                    PrecioUnitario = dto.PrecioUnitario,
                    SubTotal = dto.PrecioUnitario.HasValue ? dto.Cantidad * dto.PrecioUnitario.Value : null,
                    Nota = dto.Nota,
                    IdCliente = dto.IdCliente,
                    IdUsuario = dto.IdUsuario,
                    IdTipoBien = dto.IdTipoBien,
                    Estado = "AC",
                    FechaRegistro = DateTime.Now,
                    UserRegistro = "system", // TODO: usuario autenticado real
                };

                await _repository.AddAsync(entrega);
                await _repository.SaveChangesAsync();

                var creada = await _repository.GetByIdWithRelacionesAsync(entrega.Id);
                return BaseResponse<EntregaBienDto>.SuccessResponse(_mapper.Map<EntregaBienDto>(creada), "Entrega registrada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar la entrega");
                return BaseResponse<EntregaBienDto>.FailureResponse("Ocurrió un error al registrar la entrega.");
            }
        }
    }
}
