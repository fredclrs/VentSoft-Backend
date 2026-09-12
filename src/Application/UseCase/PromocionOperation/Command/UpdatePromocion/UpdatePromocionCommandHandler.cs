using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.PromocionOperation.Command.UpdatePromocion
{
    public class UpdatePromocionCommandHandler : IRequestHandler<UpdatePromocionCommand, BaseResponse<PromocionDto>>
    {
        private readonly IRepository<Promocion> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdatePromocionCommandHandler> _logger;

        public UpdatePromocionCommandHandler(IRepository<Promocion> repository, IMapper mapper, ILogger<UpdatePromocionCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<PromocionDto>> Handle(UpdatePromocionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var promocion = await _repository.GetByIdAsync(request.Id);
                if (promocion == null)
                    return BaseResponse<PromocionDto>.FailureResponse("No se encontró la promoción.");

                _mapper.Map(request.PromocionDto, promocion);
                promocion.UserActualizado = "system";
                promocion.FechaActualizado = DateTime.Now;

                _repository.Update(promocion);
                await _repository.SaveChangesAsync();

                return BaseResponse<PromocionDto>.SuccessResponse(_mapper.Map<PromocionDto>(promocion), "Se actualizó la promoción correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar promoción");
                return BaseResponse<PromocionDto>.FailureResponse("Ocurrió un error al actualizar la promoción.");
            }
        }
    }
}
