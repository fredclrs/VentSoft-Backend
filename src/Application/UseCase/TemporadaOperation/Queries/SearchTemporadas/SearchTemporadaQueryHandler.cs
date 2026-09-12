using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.TemporadaOperation.Queries.SearchTemporadas
{
    public class SearchTemporadaQueryHandler : IRequestHandler<SearchTemporadaQuery, BaseResponse<List<TemporadaDto>>>
    {
        private readonly IRepository<Temporada> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SearchTemporadaQueryHandler> _logger;

        public SearchTemporadaQueryHandler(IRepository<Temporada> repository, IMapper mapper, ILogger<SearchTemporadaQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<List<TemporadaDto>>> Handle(SearchTemporadaQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var lista = (await _repository.GetAllAsync()).AsEnumerable();

                if (!string.IsNullOrWhiteSpace(request.Nombre))
                    lista = lista.Where(x => x.Nombre.Contains(request.Nombre, StringComparison.OrdinalIgnoreCase));

                var resultado = _mapper.Map<List<TemporadaDto>>(lista.ToList());

                return BaseResponse<List<TemporadaDto>>.SuccessResponse(resultado, "Búsqueda realizada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar temporadas");
                return BaseResponse<List<TemporadaDto>>.FailureResponse("Ocurrió un error al buscar.");
            }
        }
    }
}
