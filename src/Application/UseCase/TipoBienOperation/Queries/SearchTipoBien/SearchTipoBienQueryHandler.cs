using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.TipoBienOperation.Queries.SearchTipoBien
{
    public class SearchTipoBienQueryHandler : IRequestHandler<SearchTipoBienQuery, BaseResponse<List<TipoBienDto>>>
    {
        private readonly IRepository<TipoBien> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SearchTipoBienQueryHandler> _logger;

        public SearchTipoBienQueryHandler(IRepository<TipoBien> repository, IMapper mapper, ILogger<SearchTipoBienQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<List<TipoBienDto>>> Handle(SearchTipoBienQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var lista = (await _repository.GetAllAsync()).Where(x => x.Estado == "AC").AsEnumerable();

                if (!string.IsNullOrWhiteSpace(request.Nombre))
                    lista = lista.Where(x => x.Nombre.Contains(request.Nombre, StringComparison.OrdinalIgnoreCase));

                var resultado = _mapper.Map<List<TipoBienDto>>(lista.ToList());

                return BaseResponse<List<TipoBienDto>>.SuccessResponse(resultado, "Búsqueda realizada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar tipos de bien");
                return BaseResponse<List<TipoBienDto>>.FailureResponse("Ocurrió un error al buscar.");
            }
        }
    }
}
