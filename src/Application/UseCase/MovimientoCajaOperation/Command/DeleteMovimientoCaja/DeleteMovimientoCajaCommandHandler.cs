using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.MovimientoCajaOperation.Command.DeleteMovimientoCaja
{
    /// <summary>Baja lógica de un movimiento de caja cargado por error — no se borra físico,
    /// por trazabilidad (igual que Cliente/Proveedor/Artículo).</summary>
    public class DeleteMovimientoCajaCommandHandler : IRequestHandler<DeleteMovimientoCajaCommand, BaseResponse<MovimientoCajaDto>>
    {
        private readonly IRepository<MovimientoCaja> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteMovimientoCajaCommandHandler> _logger;

        public DeleteMovimientoCajaCommandHandler(IRepository<MovimientoCaja> repository, IMapper mapper, ILogger<DeleteMovimientoCajaCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<MovimientoCajaDto>> Handle(DeleteMovimientoCajaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var movimiento = await _repository.GetByIdAsync(request.Id);
                if (movimiento == null)
                    return BaseResponse<MovimientoCajaDto>.FailureResponse("Movimiento de caja no encontrado.");

                movimiento.Estado = "IN";
                movimiento.UserBaja = "system"; // TODO: usuario autenticado real
                movimiento.FechaBaja = DateTime.Now;

                _repository.Update(movimiento);
                await _repository.SaveChangesAsync();

                return BaseResponse<MovimientoCajaDto>.SuccessResponse(_mapper.Map<MovimientoCajaDto>(movimiento), "Movimiento de caja eliminado correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el movimiento de caja");
                return BaseResponse<MovimientoCajaDto>.FailureResponse("Ocurrió un error al eliminar el movimiento de caja.");
            }
        }
    }
}
