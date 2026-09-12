using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.PromocionOperation.Queries.GetPromocionById
{
    public class GetPromocionByIdQueryHandler : IRequestHandler<GetPromocionByIdQuery, BaseResponse<PromocionDto>>
    {
        private readonly IRepository<Promocion> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPromocionByIdQueryHandler> _logger;

        public GetPromocionByIdQueryHandler(IRepository<Promocion> repository, IMapper mapper, ILogger<GetPromocionByIdQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<PromocionDto>> Handle(GetPromocionByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var promocion = await _repository.GetByIdAsync(request.Id);
                if (promocion == null)
                    return BaseResponse<PromocionDto>.FailureResponse("No se encontró la promoción.");

                return BaseResponse<PromocionDto>.SuccessResponse(_mapper.Map<PromocionDto>(promocion), "Se encontró la promoción correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener promoción");
                return BaseResponse<PromocionDto>.FailureResponse("Ocurrió un error al obtener la promoción.");
            }
        }
    }
}
