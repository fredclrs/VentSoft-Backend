using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.EntregaBienOperation.Queries.GetEntregasByCliente
{
    public class GetEntregasByClienteQueryHandler : IRequestHandler<GetEntregasByClienteQuery, BaseResponse<List<EntregaBienDto>>>
    {
        private readonly IEntregaBienRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetEntregasByClienteQueryHandler> _logger;

        public GetEntregasByClienteQueryHandler(IEntregaBienRepository repository, IMapper mapper, ILogger<GetEntregasByClienteQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<List<EntregaBienDto>>> Handle(GetEntregasByClienteQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entregas = await _repository.GetByClienteAsync(request.IdCliente);
                return BaseResponse<List<EntregaBienDto>>.SuccessResponse(_mapper.Map<List<EntregaBienDto>>(entregas), "Entregas encontradas correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las entregas del cliente");
                return BaseResponse<List<EntregaBienDto>>.FailureResponse("Ocurrió un error al obtener las entregas del cliente.");
            }
        }
    }
}
