using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.FamiliaOperation.Queries.GetFamiliaById
{
    public class GetFamiliaByIdQueryHandler : IRequestHandler<GetFamiliaByIdQuery, BaseResponse<FamiliaDto>>
    {
        private readonly IRepository<Familia> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetFamiliaByIdQueryHandler> _logger;

        public GetFamiliaByIdQueryHandler(IRepository<Familia> repository, IMapper mapper, ILogger<GetFamiliaByIdQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<FamiliaDto>> Handle(GetFamiliaByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var familia = await _repository.GetByIdAsync(request.Id);
                if (familia == null)
                    return BaseResponse<FamiliaDto>.FailureResponse("No se encontró la familia.");

                return BaseResponse<FamiliaDto>.SuccessResponse(_mapper.Map<FamiliaDto>(familia), "Se encontró la familia correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener familia");
                return BaseResponse<FamiliaDto>.FailureResponse("Ocurrió un error al obtener la familia.");
            }
        }
    }
}
