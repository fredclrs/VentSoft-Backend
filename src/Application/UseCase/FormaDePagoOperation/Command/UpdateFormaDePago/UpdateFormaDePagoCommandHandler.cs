using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.FormaDePagoOperation.Command.UpdateFormaDePago
{
    public class UpdateFormaDePagoCommandHandler : IRequestHandler<UpdateFormaDePagoCommand, BaseResponse<FormaDePagoDto>>
    {
        private readonly IRepository<FormaDePago> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateFormaDePagoCommandHandler> _logger;

        public UpdateFormaDePagoCommandHandler(IRepository<FormaDePago> repository, IMapper mapper, ILogger<UpdateFormaDePagoCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<FormaDePagoDto>> Handle(UpdateFormaDePagoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var formaDePago = await _repository.GetByIdAsync(request.Id);
                if (formaDePago == null)
                    return BaseResponse<FormaDePagoDto>.FailureResponse("No se encontró la forma de pago.");

                _mapper.Map(request.FormaDePagoDto, formaDePago);
                formaDePago.UserActualizado = "system";
                formaDePago.FechaActualizado = DateTime.Now;

                _repository.Update(formaDePago);
                await _repository.SaveChangesAsync();

                return BaseResponse<FormaDePagoDto>.SuccessResponse(_mapper.Map<FormaDePagoDto>(formaDePago), "Se actualizó la forma de pago correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar forma de pago");
                return BaseResponse<FormaDePagoDto>.FailureResponse("Ocurrió un error al actualizar la forma de pago.");
            }
        }
    }
}
