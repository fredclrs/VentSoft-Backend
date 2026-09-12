using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.VentaOperation.Queries.GetVentaById
{
    public class GetVentaByIdQueryHandler : IRequestHandler<GetVentaByIdQuery, BaseResponse<VentaDto>>
    {
        private readonly IVentaRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetVentaByIdQueryHandler> _logger;

        public GetVentaByIdQueryHandler(IVentaRepository repository, IMapper mapper, ILogger<GetVentaByIdQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<VentaDto>> Handle(GetVentaByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var venta = await _repository.GetByIdWithDetallesAsync(request.Id);
                if (venta == null)
                    return BaseResponse<VentaDto>.FailureResponse("Venta no encontrada.");

                return BaseResponse<VentaDto>.SuccessResponse(_mapper.Map<VentaDto>(venta), "Venta encontrada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la venta");
                return BaseResponse<VentaDto>.FailureResponse("Ocurrió un error al obtener la venta.");
            }
        }
    }
}
