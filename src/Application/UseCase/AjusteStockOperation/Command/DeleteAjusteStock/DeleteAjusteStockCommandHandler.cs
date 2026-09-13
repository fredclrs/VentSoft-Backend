using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.AjusteStockOperation.Command.DeleteAjusteStock
{
    /// <summary>Baja lógica de un ajuste de stock cargado por error — no se borra físico, por
    /// trazabilidad (igual que MovimientoCaja/Cliente/Proveedor/Artículo).</summary>
    public class DeleteAjusteStockCommandHandler : IRequestHandler<DeleteAjusteStockCommand, BaseResponse<AjusteStockDto>>
    {
        private readonly IRepository<AjusteStock> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteAjusteStockCommandHandler> _logger;

        public DeleteAjusteStockCommandHandler(IRepository<AjusteStock> repository, IMapper mapper, ILogger<DeleteAjusteStockCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<AjusteStockDto>> Handle(DeleteAjusteStockCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var ajuste = await _repository.GetByIdAsync(request.Id);
                if (ajuste == null)
                    return BaseResponse<AjusteStockDto>.FailureResponse("Ajuste de stock no encontrado.");

                ajuste.Estado = "IN";
                ajuste.UserBaja = "system"; // TODO: usuario autenticado real
                ajuste.FechaBaja = DateTime.Now;

                _repository.Update(ajuste);
                await _repository.SaveChangesAsync();

                return BaseResponse<AjusteStockDto>.SuccessResponse(_mapper.Map<AjusteStockDto>(ajuste), "Ajuste de stock eliminado correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el ajuste de stock");
                return BaseResponse<AjusteStockDto>.FailureResponse("Ocurrió un error al eliminar el ajuste de stock.");
            }
        }
    }
}
