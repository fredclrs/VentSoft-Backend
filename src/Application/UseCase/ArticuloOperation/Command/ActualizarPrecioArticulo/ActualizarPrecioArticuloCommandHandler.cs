using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.ArticuloOperation.Command.ActualizarPrecioArticulo
{
    public class ActualizarPrecioArticuloCommandHandler : IRequestHandler<ActualizarPrecioArticuloCommand, BaseResponse<ArticuloDto>>
    {
        private readonly IArticuloRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ActualizarPrecioArticuloCommandHandler> _logger;

        public ActualizarPrecioArticuloCommandHandler(IArticuloRepository repository, IMapper mapper, ILogger<ActualizarPrecioArticuloCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<ArticuloDto>> Handle(ActualizarPrecioArticuloCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var articulo = await _repository.GetByIdAsync(request.IdArticulo);
                if (articulo == null)
                    return BaseResponse<ArticuloDto>.FailureResponse("Artículo no encontrado.");

                articulo.Precio = request.Precio;
                articulo.UserActualizado = "system"; // TODO: usuario autenticado real
                articulo.FechaActualizado = DateTime.Now;

                _repository.Update(articulo);
                await _repository.SaveChangesAsync();

                return BaseResponse<ArticuloDto>.SuccessResponse(_mapper.Map<ArticuloDto>(articulo), "Precio actualizado correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el precio del artículo");
                return BaseResponse<ArticuloDto>.FailureResponse("Ocurrió un error al actualizar el precio del artículo.");
            }
        }
    }
}
