using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.VentaOperation.Queries.GetVentasByCliente
{
    public class GetVentasByClienteQueryHandler : IRequestHandler<GetVentasByClienteQuery, BaseResponse<List<VentaDto>>>
    {
        private readonly IVentaRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetVentasByClienteQueryHandler> _logger;

        public GetVentasByClienteQueryHandler(IVentaRepository repository, IMapper mapper, ILogger<GetVentasByClienteQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<List<VentaDto>>> Handle(GetVentasByClienteQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var ventas = await _repository.GetByClienteAsync(request.IdCliente);
                return BaseResponse<List<VentaDto>>.SuccessResponse(_mapper.Map<List<VentaDto>>(ventas), "Ventas encontradas correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener ventas del cliente");
                return BaseResponse<List<VentaDto>>.FailureResponse("Ocurrió un error al obtener las ventas del cliente.");
            }
        }
    }
}
