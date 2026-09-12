using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.DevolucionOperation.Queries.GetDevolucionesByCliente
{
    public class GetDevolucionesByClienteQueryHandler : IRequestHandler<GetDevolucionesByClienteQuery, BaseResponse<List<DevolucionVentaDto>>>
    {
        private readonly IDevolucionVentaRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetDevolucionesByClienteQueryHandler> _logger;

        public GetDevolucionesByClienteQueryHandler(IDevolucionVentaRepository repository, IMapper mapper, ILogger<GetDevolucionesByClienteQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<List<DevolucionVentaDto>>> Handle(GetDevolucionesByClienteQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var devoluciones = await _repository.GetByClienteAsync(request.IdCliente);
                return BaseResponse<List<DevolucionVentaDto>>.SuccessResponse(_mapper.Map<List<DevolucionVentaDto>>(devoluciones), "Devoluciones encontradas correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las devoluciones del cliente");
                return BaseResponse<List<DevolucionVentaDto>>.FailureResponse("Ocurrió un error al obtener las devoluciones del cliente.");
            }
        }
    }
}
