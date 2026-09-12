using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.FormaDePagoOperation.Command.DeleteFormaDePago
{
    public class DeleteFormaDePagoCommandHandler : IRequestHandler<DeleteFormaDePagoCommand, BaseResponse<FormaDePagoDto>>
    {
        private readonly IRepository<FormaDePago> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteFormaDePagoCommandHandler> _logger;

        public DeleteFormaDePagoCommandHandler(IRepository<FormaDePago> repository, IMapper mapper, ILogger<DeleteFormaDePagoCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<FormaDePagoDto>> Handle(DeleteFormaDePagoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var formaDePago = await _repository.GetByIdAsync(request.Id);
                if (formaDePago == null)
                    return BaseResponse<FormaDePagoDto>.FailureResponse("No se encontró la forma de pago.");

                formaDePago.Estado = "IN";
                formaDePago.UserBaja = "system";
                formaDePago.FechaBaja = DateTime.Now;

                _repository.Update(formaDePago);
                await _repository.SaveChangesAsync();

                return BaseResponse<FormaDePagoDto>.SuccessResponse(_mapper.Map<FormaDePagoDto>(formaDePago), "Se dio de baja la forma de pago correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al dar de baja forma de pago");
                return BaseResponse<FormaDePagoDto>.FailureResponse("Ocurrió un error al dar de baja la forma de pago.");
            }
        }
    }
}
