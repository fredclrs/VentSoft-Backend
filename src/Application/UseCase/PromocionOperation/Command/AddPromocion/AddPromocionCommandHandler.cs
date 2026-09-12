using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.PromocionOperation.Command.AddPromocion
{
    public class AddPromocionCommandHandler : IRequestHandler<AddPromocionCommand, BaseResponse<PromocionDto>>
    {
        private readonly IRepository<Promocion> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<AddPromocionCommandHandler> _logger;

        public AddPromocionCommandHandler(IRepository<Promocion> repository, IMapper mapper, ILogger<AddPromocionCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<PromocionDto>> Handle(AddPromocionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var promocion = _mapper.Map<Promocion>(request.PromocionDto);
                promocion.Estado = "AC";
                promocion.FechaRegistro = DateTime.Now;

                await _repository.AddAsync(promocion);
                await _repository.SaveChangesAsync();

                return BaseResponse<PromocionDto>.SuccessResponse(_mapper.Map<PromocionDto>(promocion), "Se agregó la promoción correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar promoción");
                return BaseResponse<PromocionDto>.FailureResponse("Ocurrió un error al agregar la promoción.");
            }
        }
    }
}
