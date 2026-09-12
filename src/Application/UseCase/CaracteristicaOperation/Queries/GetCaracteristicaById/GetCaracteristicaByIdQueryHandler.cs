using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.CaracteristicaOperation.Queries.GetCaracteristicaById
{
    public class GetCaracteristicaByIdQueryHandler : IRequestHandler<GetCaracteristicaByIdQuery, BaseResponse<CaracteristicaDto>>
    {
        private readonly IRepository<Caracteristica> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCaracteristicaByIdQueryHandler> _logger;

        public GetCaracteristicaByIdQueryHandler(IRepository<Caracteristica> repository, IMapper mapper, ILogger<GetCaracteristicaByIdQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<CaracteristicaDto>> Handle(GetCaracteristicaByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var caracteristica = await _repository.GetByIdAsync(request.Id);
                if (caracteristica == null)
                    return BaseResponse<CaracteristicaDto>.FailureResponse("No se encontró la característica.");

                return BaseResponse<CaracteristicaDto>.SuccessResponse(_mapper.Map<CaracteristicaDto>(caracteristica), "Se encontró la característica correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener característica");
                return BaseResponse<CaracteristicaDto>.FailureResponse("Ocurrió un error al obtener la característica.");
            }
        }
    }
}
