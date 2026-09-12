using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.FormaDePagoOperation.Queries.SearchFormaDePagos
{
    public class SearchFormaDePagoQueryHandler : IRequestHandler<SearchFormaDePagoQuery, BaseResponse<List<FormaDePagoDto>>>
    {
        private readonly IRepository<FormaDePago> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SearchFormaDePagoQueryHandler> _logger;

        public SearchFormaDePagoQueryHandler(IRepository<FormaDePago> repository, IMapper mapper, ILogger<SearchFormaDePagoQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<List<FormaDePagoDto>>> Handle(SearchFormaDePagoQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var lista = (await _repository.GetAllAsync()).AsEnumerable();

                if (!string.IsNullOrWhiteSpace(request.Nombre))
                    lista = lista.Where(x => x.Nombre.Contains(request.Nombre, StringComparison.OrdinalIgnoreCase));

                var resultado = _mapper.Map<List<FormaDePagoDto>>(lista.ToList());

                return BaseResponse<List<FormaDePagoDto>>.SuccessResponse(resultado, "Búsqueda realizada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar forma de pagos");
                return BaseResponse<List<FormaDePagoDto>>.FailureResponse("Ocurrió un error al buscar.");
            }
        }
    }
}
