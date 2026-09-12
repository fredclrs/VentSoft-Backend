using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.MovimientoCajaOperation.Queries.GetMovimientosDelDia
{
    public class GetMovimientosDelDiaQueryHandler : IRequestHandler<GetMovimientosDelDiaQuery, BaseResponse<List<MovimientoCajaDto>>>
    {
        private readonly IRepository<MovimientoCaja> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetMovimientosDelDiaQueryHandler> _logger;

        public GetMovimientosDelDiaQueryHandler(IRepository<MovimientoCaja> repository, IMapper mapper, ILogger<GetMovimientosDelDiaQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<List<MovimientoCajaDto>>> Handle(GetMovimientosDelDiaQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Tabla chica (negocio pequeño): se filtra en memoria, mismo criterio que el
                // resto de las búsquedas simples del sistema.
                var movimientos = (await _repository.GetAllAsync())
                    .Where(m => m.Estado == "AC" && m.Fecha.Date == request.Fecha.Date)
                    .OrderBy(m => m.Id)
                    .ToList();

                return BaseResponse<List<MovimientoCajaDto>>.SuccessResponse(_mapper.Map<List<MovimientoCajaDto>>(movimientos), "Movimientos de caja del día encontrados correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los movimientos de caja del día");
                return BaseResponse<List<MovimientoCajaDto>>.FailureResponse("Ocurrió un error al obtener los movimientos de caja del día.");
            }
        }
    }
}
