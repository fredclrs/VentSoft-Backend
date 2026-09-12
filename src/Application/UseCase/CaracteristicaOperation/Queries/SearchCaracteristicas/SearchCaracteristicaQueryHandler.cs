using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.CaracteristicaOperation.Queries.SearchCaracteristicas
{
    public class SearchCaracteristicaQueryHandler : IRequestHandler<SearchCaracteristicaQuery, BaseResponse<List<CaracteristicaDto>>>
    {
        private readonly IRepository<Caracteristica> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SearchCaracteristicaQueryHandler> _logger;

        public SearchCaracteristicaQueryHandler(IRepository<Caracteristica> repository, IMapper mapper, ILogger<SearchCaracteristicaQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<List<CaracteristicaDto>>> Handle(SearchCaracteristicaQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var lista = (await _repository.GetAllAsync()).AsEnumerable();

                if (!string.IsNullOrWhiteSpace(request.NombreCaracteristica))
                    lista = lista.Where(x => x.NombreCaracteristica.Contains(request.NombreCaracteristica, StringComparison.OrdinalIgnoreCase));

                var resultado = _mapper.Map<List<CaracteristicaDto>>(lista.ToList());

                return BaseResponse<List<CaracteristicaDto>>.SuccessResponse(resultado, "Búsqueda realizada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar características");
                return BaseResponse<List<CaracteristicaDto>>.FailureResponse("Ocurrió un error al buscar.");
            }
        }
    }
}
