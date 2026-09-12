using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.CaracteristicaOperation.Command.UpdateCaracteristica
{
    public class UpdateCaracteristicaCommandHandler : IRequestHandler<UpdateCaracteristicaCommand, BaseResponse<CaracteristicaDto>>
    {
        private readonly IRepository<Caracteristica> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCaracteristicaCommandHandler> _logger;

        public UpdateCaracteristicaCommandHandler(IRepository<Caracteristica> repository, IMapper mapper, ILogger<UpdateCaracteristicaCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<CaracteristicaDto>> Handle(UpdateCaracteristicaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var caracteristica = await _repository.GetByIdAsync(request.Id);
                if (caracteristica == null)
                    return BaseResponse<CaracteristicaDto>.FailureResponse("No se encontró la característica.");

                _mapper.Map(request.CaracteristicaDto, caracteristica);
                caracteristica.UserActualizado = "system";
                caracteristica.FechaActualizado = DateTime.Now;

                _repository.Update(caracteristica);
                await _repository.SaveChangesAsync();

                return BaseResponse<CaracteristicaDto>.SuccessResponse(_mapper.Map<CaracteristicaDto>(caracteristica), "Se actualizó la característica correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar característica");
                return BaseResponse<CaracteristicaDto>.FailureResponse("Ocurrió un error al actualizar la característica.");
            }
        }
    }
}
