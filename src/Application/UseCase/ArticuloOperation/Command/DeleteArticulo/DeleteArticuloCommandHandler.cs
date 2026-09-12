using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.ArticuloOperation.Command.DeleteArticulo
{
    public class DeleteArticuloCommandHandler : IRequestHandler<DeleteArticuloCommand, BaseResponse<ArticuloDto>>
    {
        private readonly IArticuloRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteArticuloCommandHandler> _logger;

        public DeleteArticuloCommandHandler(IArticuloRepository repository, IMapper mapper, ILogger<DeleteArticuloCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<ArticuloDto>> Handle(DeleteArticuloCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var articulo = await _repository.GetByIdAsync(request.Id);
                if (articulo == null)
                    return BaseResponse<ArticuloDto>.FailureResponse("Artículo no encontrado.");

                // Baja lógica: el artículo puede tener Compras/Ventas asociadas.
                articulo.Estado = "IN";
                articulo.UserBaja = "system";
                articulo.FechaBaja = DateTime.Now;

                _repository.Update(articulo);
                await _repository.SaveChangesAsync();

                return BaseResponse<ArticuloDto>.SuccessResponse(_mapper.Map<ArticuloDto>(articulo), "Artículo dado de baja correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al dar de baja el artículo");
                return BaseResponse<ArticuloDto>.FailureResponse("Ocurrió un error al dar de baja el artículo.");
            }
        }
    }
}
