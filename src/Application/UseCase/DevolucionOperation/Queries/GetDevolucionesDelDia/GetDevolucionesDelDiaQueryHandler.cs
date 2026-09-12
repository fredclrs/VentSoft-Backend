using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.DevolucionOperation.Queries.GetDevolucionesDelDia
{
    public class GetDevolucionesDelDiaQueryHandler : IRequestHandler<GetDevolucionesDelDiaQuery, BaseResponse<List<DevolucionVentaDto>>>
    {
        private readonly IDevolucionVentaRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetDevolucionesDelDiaQueryHandler> _logger;

        public GetDevolucionesDelDiaQueryHandler(IDevolucionVentaRepository repository, IMapper mapper, ILogger<GetDevolucionesDelDiaQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<List<DevolucionVentaDto>>> Handle(GetDevolucionesDelDiaQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var devoluciones = await _repository.GetDelDiaAsync(request.Fecha);
                return BaseResponse<List<DevolucionVentaDto>>.SuccessResponse(_mapper.Map<List<DevolucionVentaDto>>(devoluciones), "Devoluciones del día encontradas correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las devoluciones del día");
                return BaseResponse<List<DevolucionVentaDto>>.FailureResponse("Ocurrió un error al obtener las devoluciones del día.");
            }
        }
    }
}
