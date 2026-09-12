using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.TemporadaOperation.Command.UpdateTemporada
{
    public class UpdateTemporadaCommandHandler : IRequestHandler<UpdateTemporadaCommand, BaseResponse<TemporadaDto>>
    {
        private readonly IRepository<Temporada> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateTemporadaCommandHandler> _logger;

        public UpdateTemporadaCommandHandler(IRepository<Temporada> repository, IMapper mapper, ILogger<UpdateTemporadaCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<TemporadaDto>> Handle(UpdateTemporadaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var temporada = await _repository.GetByIdAsync(request.Id);
                if (temporada == null)
                    return BaseResponse<TemporadaDto>.FailureResponse("No se encontró la temporada.");

                _mapper.Map(request.TemporadaDto, temporada);
                temporada.UserActualizado = "system";
                temporada.FechaActualizado = DateTime.Now;

                _repository.Update(temporada);
                await _repository.SaveChangesAsync();

                return BaseResponse<TemporadaDto>.SuccessResponse(_mapper.Map<TemporadaDto>(temporada), "Se actualizó la temporada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar temporada");
                return BaseResponse<TemporadaDto>.FailureResponse("Ocurrió un error al actualizar la temporada.");
            }
        }
    }
}
