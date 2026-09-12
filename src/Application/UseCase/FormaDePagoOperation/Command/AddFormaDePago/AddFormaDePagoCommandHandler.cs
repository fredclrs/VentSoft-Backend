using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.FormaDePagoOperation.Command.AddFormaDePago
{
    public class AddFormaDePagoCommandHandler : IRequestHandler<AddFormaDePagoCommand, BaseResponse<FormaDePagoDto>>
    {
        private readonly IRepository<FormaDePago> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<AddFormaDePagoCommandHandler> _logger;

        public AddFormaDePagoCommandHandler(IRepository<FormaDePago> repository, IMapper mapper, ILogger<AddFormaDePagoCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<FormaDePagoDto>> Handle(AddFormaDePagoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var formaDePago = _mapper.Map<FormaDePago>(request.FormaDePagoDto);
                formaDePago.Estado = "AC";
                formaDePago.FechaRegistro = DateTime.Now;

                await _repository.AddAsync(formaDePago);
                await _repository.SaveChangesAsync();

                return BaseResponse<FormaDePagoDto>.SuccessResponse(_mapper.Map<FormaDePagoDto>(formaDePago), "Se agregó la forma de pago correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar forma de pago");
                return BaseResponse<FormaDePagoDto>.FailureResponse("Ocurrió un error al agregar la forma de pago.");
            }
        }
    }
}
