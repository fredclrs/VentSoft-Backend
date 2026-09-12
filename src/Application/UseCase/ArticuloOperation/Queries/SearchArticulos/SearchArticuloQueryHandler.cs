using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.ArticuloOperation.Queries.SearchArticulos
{
    public class SearchArticuloQueryHandler : IRequestHandler<SearchArticuloQuery, BaseResponse<List<ArticuloDto>>>
    {
        private readonly IArticuloRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SearchArticuloQueryHandler> _logger;

        public SearchArticuloQueryHandler(IArticuloRepository repository, IMapper mapper, ILogger<SearchArticuloQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<List<ArticuloDto>>> Handle(SearchArticuloQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var articulos = (await _repository.GetAllWithCaracteristicasAsync()).AsEnumerable();

                if (!string.IsNullOrWhiteSpace(request.Codigo))
                    articulos = articulos.Where(a => a.Codigo.Contains(request.Codigo, StringComparison.OrdinalIgnoreCase));

                if (!string.IsNullOrWhiteSpace(request.Descripcion))
                    articulos = articulos.Where(a => a.Descripcion != null && a.Descripcion.Contains(request.Descripcion, StringComparison.OrdinalIgnoreCase));

                if (request.IdFamilia.HasValue)
                    articulos = articulos.Where(a => a.IdFamilia == request.IdFamilia.Value);

                var resultado = _mapper.Map<List<ArticuloDto>>(articulos.ToList());

                return BaseResponse<List<ArticuloDto>>.SuccessResponse(resultado, "Artículos encontrados correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar artículos");
                return BaseResponse<List<ArticuloDto>>.FailureResponse("Ocurrió un error al buscar artículos.");
            }
        }
    }
}
