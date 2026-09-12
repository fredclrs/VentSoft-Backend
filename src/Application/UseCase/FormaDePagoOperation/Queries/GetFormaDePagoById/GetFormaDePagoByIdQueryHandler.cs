using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.FormaDePagoOperation.Queries.GetFormaDePagoById
{
    public class GetFormaDePagoByIdQueryHandler : IRequestHandler<GetFormaDePagoByIdQuery, BaseResponse<FormaDePagoDto>>
    {
        private readonly IRepository<FormaDePago> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetFormaDePagoByIdQueryHandler> _logger;

        public GetFormaDePagoByIdQueryHandler(IRepository<FormaDePago> repository, IMapper mapper, ILogger<GetFormaDePagoByIdQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<FormaDePagoDto>> Handle(GetFormaDePagoByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var formaDePago = await _repository.GetByIdAsync(request.Id);
                if (formaDePago == null)
                    return BaseResponse<FormaDePagoDto>.FailureResponse("No se encontró la forma de pago.");

                return BaseResponse<FormaDePagoDto>.SuccessResponse(_mapper.Map<FormaDePagoDto>(formaDePago), "Se encontró la forma de pago correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener forma de pago");
                return BaseResponse<FormaDePagoDto>.FailureResponse("Ocurrió un error al obtener la forma de pago.");
            }
        }
    }
}
