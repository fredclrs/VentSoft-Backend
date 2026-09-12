using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.PagoOperation.Queries.GetPagosByProveedor
{
    public class GetPagosByProveedorQueryHandler : IRequestHandler<GetPagosByProveedorQuery, BaseResponse<List<PagoDto>>>
    {
        private readonly IRepository<Pago> _pagoRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPagosByProveedorQueryHandler> _logger;

        public GetPagosByProveedorQueryHandler(IRepository<Pago> pagoRepository, IMapper mapper, ILogger<GetPagosByProveedorQueryHandler> logger)
        {
            _pagoRepository = pagoRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<List<PagoDto>>> Handle(GetPagosByProveedorQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var pagos = (await _pagoRepository.GetAllAsync())
                    .Where(p => p.IdProveedor == request.IdProveedor)
                    .OrderByDescending(p => p.Fecha)
                    .ToList();

                return BaseResponse<List<PagoDto>>.SuccessResponse(_mapper.Map<List<PagoDto>>(pagos), "Pagos encontrados correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener pagos del proveedor");
                return BaseResponse<List<PagoDto>>.FailureResponse("Ocurrió un error al obtener los pagos del proveedor.");
            }
        }
    }
}
