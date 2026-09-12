using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.PagoOperation.Queries.GetDeudaProveedor
{
    public class GetDeudaProveedorQueryHandler : IRequestHandler<GetDeudaProveedorQuery, BaseResponse<ProveedorDeudaDto>>
    {
        private readonly IRepository<Proveedor> _proveedorRepository;
        private readonly ICompraRepository _compraRepository;
        private readonly ILogger<GetDeudaProveedorQueryHandler> _logger;

        public GetDeudaProveedorQueryHandler(IRepository<Proveedor> proveedorRepository, ICompraRepository compraRepository, ILogger<GetDeudaProveedorQueryHandler> logger)
        {
            _proveedorRepository = proveedorRepository;
            _compraRepository = compraRepository;
            _logger = logger;
        }

        public async Task<BaseResponse<ProveedorDeudaDto>> Handle(GetDeudaProveedorQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var proveedor = await _proveedorRepository.GetByIdAsync(request.IdProveedor);
                if (proveedor == null)
                    return BaseResponse<ProveedorDeudaDto>.FailureResponse("Proveedor no encontrado.");

                var deuda = (await _compraRepository.GetByProveedorAsync(request.IdProveedor))
                    .Where(c => c.Estado == "AC")
                    .Sum(c => c.PorPagar);

                var dto = new ProveedorDeudaDto
                {
                    IdProveedor = proveedor.Id,
                    NombreProveedor = proveedor.Nombre,
                    DeudaActual = deuda
                };

                return BaseResponse<ProveedorDeudaDto>.SuccessResponse(dto, "Deuda consultada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar la deuda del proveedor");
                return BaseResponse<ProveedorDeudaDto>.FailureResponse("Ocurrió un error al consultar la deuda del proveedor.");
            }
        }
    }
}
