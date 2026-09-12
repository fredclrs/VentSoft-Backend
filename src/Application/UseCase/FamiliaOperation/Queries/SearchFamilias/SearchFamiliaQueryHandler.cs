using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.FamiliaOperation.Queries.SearchFamilias
{
    public class SearchFamiliaQueryHandler : IRequestHandler<SearchFamiliaQuery, BaseResponse<List<FamiliaDto>>>
    {
        private readonly IRepository<Familia> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SearchFamiliaQueryHandler> _logger;

        public SearchFamiliaQueryHandler(IRepository<Familia> repository, IMapper mapper, ILogger<SearchFamiliaQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<List<FamiliaDto>>> Handle(SearchFamiliaQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var lista = (await _repository.GetAllAsync()).AsEnumerable();

                if (!string.IsNullOrWhiteSpace(request.NombreFamilia))
                    lista = lista.Where(x => x.NombreFamilia.Contains(request.NombreFamilia, StringComparison.OrdinalIgnoreCase));

                var resultado = _mapper.Map<List<FamiliaDto>>(lista.ToList());

                return BaseResponse<List<FamiliaDto>>.SuccessResponse(resultado, "Búsqueda realizada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar familias");
                return BaseResponse<List<FamiliaDto>>.FailureResponse("Ocurrió un error al buscar.");
            }
        }
    }
}
