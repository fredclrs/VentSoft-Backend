using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.TipoBienOperation.Command.UpdateTipoBien
{
    public class UpdateTipoBienCommandHandler : IRequestHandler<UpdateTipoBienCommand, BaseResponse<TipoBienDto>>
    {
        private readonly IRepository<TipoBien> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateTipoBienCommandHandler> _logger;

        public UpdateTipoBienCommandHandler(IRepository<TipoBien> repository, IMapper mapper, ILogger<UpdateTipoBienCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<TipoBienDto>> Handle(UpdateTipoBienCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var tipoBien = await _repository.GetByIdAsync(request.Id);
                if (tipoBien == null)
                    return BaseResponse<TipoBienDto>.FailureResponse("No se encontró el tipo de bien.");

                _mapper.Map(request.TipoBienDto, tipoBien);
                tipoBien.UserActualizado = "system";
                tipoBien.FechaActualizado = DateTime.Now;

                _repository.Update(tipoBien);
                await _repository.SaveChangesAsync();

                return BaseResponse<TipoBienDto>.SuccessResponse(_mapper.Map<TipoBienDto>(tipoBien), "Se actualizó el tipo de bien correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar tipo de bien");
                return BaseResponse<TipoBienDto>.FailureResponse("Ocurrió un error al actualizar el tipo de bien.");
            }
        }
    }
}
