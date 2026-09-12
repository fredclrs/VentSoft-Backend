using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.TemporadaOperation.Command.DeleteTemporada
{
    public class DeleteTemporadaCommandHandler : IRequestHandler<DeleteTemporadaCommand, BaseResponse<TemporadaDto>>
    {
        private readonly IRepository<Temporada> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteTemporadaCommandHandler> _logger;

        public DeleteTemporadaCommandHandler(IRepository<Temporada> repository, IMapper mapper, ILogger<DeleteTemporadaCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<TemporadaDto>> Handle(DeleteTemporadaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var temporada = await _repository.GetByIdAsync(request.Id);
                if (temporada == null)
                    return BaseResponse<TemporadaDto>.FailureResponse("No se encontró la temporada.");

                temporada.Estado = "IN";
                temporada.UserBaja = "system";
                temporada.FechaBaja = DateTime.Now;

                _repository.Update(temporada);
                await _repository.SaveChangesAsync();

                return BaseResponse<TemporadaDto>.SuccessResponse(_mapper.Map<TemporadaDto>(temporada), "Se dio de baja la temporada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al dar de baja temporada");
                return BaseResponse<TemporadaDto>.FailureResponse("Ocurrió un error al dar de baja la temporada.");
            }
        }
    }
}
