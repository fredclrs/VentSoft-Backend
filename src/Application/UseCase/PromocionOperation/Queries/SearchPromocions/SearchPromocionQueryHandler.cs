using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.PromocionOperation.Queries.SearchPromocions
{
    public class SearchPromocionQueryHandler : IRequestHandler<SearchPromocionQuery, BaseResponse<List<PromocionDto>>>
    {
        private readonly IRepository<Promocion> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SearchPromocionQueryHandler> _logger;

        public SearchPromocionQueryHandler(IRepository<Promocion> repository, IMapper mapper, ILogger<SearchPromocionQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<List<PromocionDto>>> Handle(SearchPromocionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var lista = (await _repository.GetAllAsync()).AsEnumerable();

                if (!string.IsNullOrWhiteSpace(request.NombrePromocion))
                    lista = lista.Where(x => x.NombrePromocion.Contains(request.NombrePromocion, StringComparison.OrdinalIgnoreCase));

                var resultado = _mapper.Map<List<PromocionDto>>(lista.ToList());

                return BaseResponse<List<PromocionDto>>.SuccessResponse(resultado, "Búsqueda realizada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar promocións");
                return BaseResponse<List<PromocionDto>>.FailureResponse("Ocurrió un error al buscar.");
            }
        }
    }
}
