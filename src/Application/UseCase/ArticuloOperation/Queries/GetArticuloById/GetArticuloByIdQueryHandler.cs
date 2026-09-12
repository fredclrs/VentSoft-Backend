using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.ArticuloOperation.Queries.GetArticuloById
{
    public class GetArticuloByIdQueryHandler : IRequestHandler<GetArticuloByIdQuery, BaseResponse<ArticuloDto>>
    {
        private readonly IArticuloRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetArticuloByIdQueryHandler> _logger;

        public GetArticuloByIdQueryHandler(IArticuloRepository repository, IMapper mapper, ILogger<GetArticuloByIdQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<ArticuloDto>> Handle(GetArticuloByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var articulo = await _repository.GetByIdWithCaracteristicasAsync(request.Id);
                if (articulo == null)
                    return BaseResponse<ArticuloDto>.FailureResponse("Artículo no encontrado.");

                return BaseResponse<ArticuloDto>.SuccessResponse(_mapper.Map<ArticuloDto>(articulo), "Artículo encontrado correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el artículo");
                return BaseResponse<ArticuloDto>.FailureResponse("Ocurrió un error al obtener el artículo.");
            }
        }
    }
}
