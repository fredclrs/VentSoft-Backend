using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.VentaOperation.Queries.GetVentasDelDia
{
    public class GetVentasDelDiaQueryHandler : IRequestHandler<GetVentasDelDiaQuery, BaseResponse<List<VentaDto>>>
    {
        private readonly IVentaRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetVentasDelDiaQueryHandler> _logger;

        public GetVentasDelDiaQueryHandler(IVentaRepository repository, IMapper mapper, ILogger<GetVentasDelDiaQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<List<VentaDto>>> Handle(GetVentasDelDiaQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var ventas = await _repository.GetByFechaAsync(request.Fecha);
                return BaseResponse<List<VentaDto>>.SuccessResponse(_mapper.Map<List<VentaDto>>(ventas), "Ventas del día encontradas correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las ventas del día");
                return BaseResponse<List<VentaDto>>.FailureResponse("Ocurrió un error al obtener las ventas del día.");
            }
        }
    }
}
