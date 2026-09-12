using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.DevolucionOperation.Queries.GetDevolucionesByVenta
{
    public class GetDevolucionesByVentaQueryHandler : IRequestHandler<GetDevolucionesByVentaQuery, BaseResponse<List<DevolucionVentaDto>>>
    {
        private readonly IDevolucionVentaRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetDevolucionesByVentaQueryHandler> _logger;

        public GetDevolucionesByVentaQueryHandler(IDevolucionVentaRepository repository, IMapper mapper, ILogger<GetDevolucionesByVentaQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<List<DevolucionVentaDto>>> Handle(GetDevolucionesByVentaQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var devoluciones = await _repository.GetByVentaAsync(request.IdVenta);
                return BaseResponse<List<DevolucionVentaDto>>.SuccessResponse(_mapper.Map<List<DevolucionVentaDto>>(devoluciones), "Devoluciones encontradas correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las devoluciones de la venta");
                return BaseResponse<List<DevolucionVentaDto>>.FailureResponse("Ocurrió un error al obtener las devoluciones de la venta.");
            }
        }
    }
}
