using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.EtiquetaPendienteOperation.Queries.GetEtiquetasPendientes
{
    public class GetEtiquetasPendientesQueryHandler : IRequestHandler<GetEtiquetasPendientesQuery, BaseResponse<List<EtiquetaPendienteDto>>>
    {
        /// <summary>Nadie tiene que acordarse de vaciar la cola a mano — lo que quede sin
        /// imprimirse más de este tiempo se descarta solo la próxima vez que alguien la abra.</summary>
        private static readonly TimeSpan AntiguedadMaxima = TimeSpan.FromDays(30);

        private readonly IRepository<EtiquetaPendiente> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetEtiquetasPendientesQueryHandler> _logger;

        public GetEtiquetasPendientesQueryHandler(IRepository<EtiquetaPendiente> repository, IMapper mapper, ILogger<GetEtiquetasPendientesQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<List<EtiquetaPendienteDto>>> Handle(GetEtiquetasPendientesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var todas = await _repository.GetAllAsync();

                var limite = DateTime.Now - AntiguedadMaxima;
                var vencidas = todas.Where(e => e.FechaAgregado < limite).ToList();
                if (vencidas.Count > 0)
                {
                    foreach (var vencida in vencidas)
                    {
                        _repository.Delete(vencida);
                    }
                    await _repository.SaveChangesAsync();
                    _logger.LogInformation("Se limpiaron {Cantidad} etiqueta(s) pendiente(s) de más de {Dias} días sin imprimirse.", vencidas.Count, AntiguedadMaxima.TotalDays);
                }

                var vigentes = todas
                    .Except(vencidas)
                    .OrderBy(e => e.FechaAgregado)
                    .ToList();

                return BaseResponse<List<EtiquetaPendienteDto>>.SuccessResponse(_mapper.Map<List<EtiquetaPendienteDto>>(vigentes), "Cola de etiquetas obtenida correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la cola de etiquetas");
                return BaseResponse<List<EtiquetaPendienteDto>>.FailureResponse("Ocurrió un error al obtener la cola de etiquetas.");
            }
        }
    }
}
