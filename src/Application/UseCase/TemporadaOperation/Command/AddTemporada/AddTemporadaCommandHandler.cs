using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.TemporadaOperation.Command.AddTemporada
{
    public class AddTemporadaCommandHandler : IRequestHandler<AddTemporadaCommand, BaseResponse<TemporadaDto>>
    {
        private readonly IRepository<Temporada> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<AddTemporadaCommandHandler> _logger;

        public AddTemporadaCommandHandler(IRepository<Temporada> repository, IMapper mapper, ILogger<AddTemporadaCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<TemporadaDto>> Handle(AddTemporadaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var temporada = _mapper.Map<Temporada>(request.TemporadaDto);
                temporada.Estado = "AC";
                temporada.FechaRegistro = DateTime.Now;

                await _repository.AddAsync(temporada);
                await _repository.SaveChangesAsync();

                return BaseResponse<TemporadaDto>.SuccessResponse(_mapper.Map<TemporadaDto>(temporada), "Se agregó la temporada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar temporada");
                return BaseResponse<TemporadaDto>.FailureResponse("Ocurrió un error al agregar la temporada.");
            }
        }
    }
}
