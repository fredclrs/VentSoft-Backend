using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.TemporadaOperation.Queries.GetTemporadaById
{
    public class GetTemporadaByIdQueryHandler : IRequestHandler<GetTemporadaByIdQuery, BaseResponse<TemporadaDto>>
    {
        private readonly IRepository<Temporada> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetTemporadaByIdQueryHandler> _logger;

        public GetTemporadaByIdQueryHandler(IRepository<Temporada> repository, IMapper mapper, ILogger<GetTemporadaByIdQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<TemporadaDto>> Handle(GetTemporadaByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var temporada = await _repository.GetByIdAsync(request.Id);
                if (temporada == null)
                    return BaseResponse<TemporadaDto>.FailureResponse("No se encontró la temporada.");

                return BaseResponse<TemporadaDto>.SuccessResponse(_mapper.Map<TemporadaDto>(temporada), "Se encontró la temporada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener temporada");
                return BaseResponse<TemporadaDto>.FailureResponse("Ocurrió un error al obtener la temporada.");
            }
        }
    }
}
