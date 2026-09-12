using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.VentaOperation.Queries.GetVentasTodas
{
    public class GetVentasTodasQueryHandler : IRequestHandler<GetVentasTodasQuery, BaseResponse<List<VentaDto>>>
    {
        private readonly IVentaRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetVentasTodasQueryHandler> _logger;

        public GetVentasTodasQueryHandler(IVentaRepository repository, IMapper mapper, ILogger<GetVentasTodasQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<List<VentaDto>>> Handle(GetVentasTodasQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var ventas = await _repository.GetTodasAsync();
                return BaseResponse<List<VentaDto>>.SuccessResponse(_mapper.Map<List<VentaDto>>(ventas), "Ventas encontradas correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las ventas");
                return BaseResponse<List<VentaDto>>.FailureResponse("Ocurrió un error al obtener las ventas.");
            }
        }
    }
}
