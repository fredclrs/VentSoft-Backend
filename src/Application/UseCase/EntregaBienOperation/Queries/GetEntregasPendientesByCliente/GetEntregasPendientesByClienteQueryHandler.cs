using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.EntregaBienOperation.Queries.GetEntregasPendientesByCliente
{
    public class GetEntregasPendientesByClienteQueryHandler : IRequestHandler<GetEntregasPendientesByClienteQuery, BaseResponse<List<EntregaBienDto>>>
    {
        private readonly IEntregaBienRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetEntregasPendientesByClienteQueryHandler> _logger;

        public GetEntregasPendientesByClienteQueryHandler(IEntregaBienRepository repository, IMapper mapper, ILogger<GetEntregasPendientesByClienteQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<List<EntregaBienDto>>> Handle(GetEntregasPendientesByClienteQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entregas = await _repository.GetPendientesByClienteAsync(request.IdCliente);
                return BaseResponse<List<EntregaBienDto>>.SuccessResponse(_mapper.Map<List<EntregaBienDto>>(entregas), "Entregas pendientes encontradas correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las entregas pendientes del cliente");
                return BaseResponse<List<EntregaBienDto>>.FailureResponse("Ocurrió un error al obtener las entregas pendientes.");
            }
        }
    }
}
