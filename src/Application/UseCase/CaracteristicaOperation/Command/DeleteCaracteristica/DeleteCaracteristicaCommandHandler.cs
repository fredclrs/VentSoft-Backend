using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.CaracteristicaOperation.Command.DeleteCaracteristica
{
    public class DeleteCaracteristicaCommandHandler : IRequestHandler<DeleteCaracteristicaCommand, BaseResponse<CaracteristicaDto>>
    {
        private readonly IRepository<Caracteristica> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteCaracteristicaCommandHandler> _logger;

        public DeleteCaracteristicaCommandHandler(IRepository<Caracteristica> repository, IMapper mapper, ILogger<DeleteCaracteristicaCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<CaracteristicaDto>> Handle(DeleteCaracteristicaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var caracteristica = await _repository.GetByIdAsync(request.Id);
                if (caracteristica == null)
                    return BaseResponse<CaracteristicaDto>.FailureResponse("No se encontró la característica.");

                caracteristica.Estado = "IN";
                caracteristica.UserBaja = "system";
                caracteristica.FechaBaja = DateTime.Now;

                _repository.Update(caracteristica);
                await _repository.SaveChangesAsync();

                return BaseResponse<CaracteristicaDto>.SuccessResponse(_mapper.Map<CaracteristicaDto>(caracteristica), "Se dio de baja la característica correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al dar de baja característica");
                return BaseResponse<CaracteristicaDto>.FailureResponse("Ocurrió un error al dar de baja la característica.");
            }
        }
    }
}
