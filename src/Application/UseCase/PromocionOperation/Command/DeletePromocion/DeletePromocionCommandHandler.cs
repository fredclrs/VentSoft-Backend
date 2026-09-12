using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.PromocionOperation.Command.DeletePromocion
{
    public class DeletePromocionCommandHandler : IRequestHandler<DeletePromocionCommand, BaseResponse<PromocionDto>>
    {
        private readonly IRepository<Promocion> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeletePromocionCommandHandler> _logger;

        public DeletePromocionCommandHandler(IRepository<Promocion> repository, IMapper mapper, ILogger<DeletePromocionCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<PromocionDto>> Handle(DeletePromocionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var promocion = await _repository.GetByIdAsync(request.Id);
                if (promocion == null)
                    return BaseResponse<PromocionDto>.FailureResponse("No se encontró la promoción.");

                promocion.Estado = "IN";
                promocion.UserBaja = "system";
                promocion.FechaBaja = DateTime.Now;

                _repository.Update(promocion);
                await _repository.SaveChangesAsync();

                return BaseResponse<PromocionDto>.SuccessResponse(_mapper.Map<PromocionDto>(promocion), "Se dio de baja la promoción correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al dar de baja promoción");
                return BaseResponse<PromocionDto>.FailureResponse("Ocurrió un error al dar de baja la promoción.");
            }
        }
    }
}
