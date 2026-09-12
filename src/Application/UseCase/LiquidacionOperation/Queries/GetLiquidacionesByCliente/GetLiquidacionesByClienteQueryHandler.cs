using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.LiquidacionOperation.Queries.GetLiquidacionesByCliente
{
    public class GetLiquidacionesByClienteQueryHandler : IRequestHandler<GetLiquidacionesByClienteQuery, BaseResponse<List<LiquidacionDto>>>
    {
        private readonly ILiquidacionRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetLiquidacionesByClienteQueryHandler> _logger;

        public GetLiquidacionesByClienteQueryHandler(ILiquidacionRepository repository, IMapper mapper, ILogger<GetLiquidacionesByClienteQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<List<LiquidacionDto>>> Handle(GetLiquidacionesByClienteQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var liquidaciones = await _repository.GetByClienteAsync(request.IdCliente);
                return BaseResponse<List<LiquidacionDto>>.SuccessResponse(_mapper.Map<List<LiquidacionDto>>(liquidaciones), "Liquidaciones encontradas correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las liquidaciones del cliente");
                return BaseResponse<List<LiquidacionDto>>.FailureResponse("Ocurrió un error al obtener las liquidaciones del cliente.");
            }
        }
    }
}
