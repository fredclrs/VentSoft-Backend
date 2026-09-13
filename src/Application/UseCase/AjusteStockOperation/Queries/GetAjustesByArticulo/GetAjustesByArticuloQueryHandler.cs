using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.AjusteStockOperation.Queries.GetAjustesByArticulo
{
    public class GetAjustesByArticuloQueryHandler : IRequestHandler<GetAjustesByArticuloQuery, BaseResponse<List<AjusteStockDto>>>
    {
        private readonly IRepository<AjusteStock> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAjustesByArticuloQueryHandler> _logger;

        public GetAjustesByArticuloQueryHandler(IRepository<AjusteStock> repository, IMapper mapper, ILogger<GetAjustesByArticuloQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<List<AjusteStockDto>>> Handle(GetAjustesByArticuloQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var ajustes = (await _repository.GetAllAsync())
                    .Where(a => a.IdArticulo == request.IdArticulo && a.Estado == "AC")
                    .OrderByDescending(a => a.Id)
                    .ToList();

                return BaseResponse<List<AjusteStockDto>>.SuccessResponse(_mapper.Map<List<AjusteStockDto>>(ajustes), "Ajustes de stock encontrados correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los ajustes de stock del artículo");
                return BaseResponse<List<AjusteStockDto>>.FailureResponse("Ocurrió un error al obtener los ajustes de stock del artículo.");
            }
        }
    }
}
