using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.CaracteristicaOperation.Command.AddCaracteristica
{
    public class AddCaracteristicaCommandHandler : IRequestHandler<AddCaracteristicaCommand, BaseResponse<CaracteristicaDto>>
    {
        private readonly IRepository<Caracteristica> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<AddCaracteristicaCommandHandler> _logger;

        public AddCaracteristicaCommandHandler(IRepository<Caracteristica> repository, IMapper mapper, ILogger<AddCaracteristicaCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<CaracteristicaDto>> Handle(AddCaracteristicaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var caracteristica = _mapper.Map<Caracteristica>(request.CaracteristicaDto);
                caracteristica.Estado = "AC";
                caracteristica.FechaRegistro = DateTime.Now;

                await _repository.AddAsync(caracteristica);
                await _repository.SaveChangesAsync();

                return BaseResponse<CaracteristicaDto>.SuccessResponse(_mapper.Map<CaracteristicaDto>(caracteristica), "Se agregó la característica correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar característica");
                return BaseResponse<CaracteristicaDto>.FailureResponse("Ocurrió un error al agregar la característica.");
            }
        }
    }
}
