using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.CompraOperation.Queries.GetComprasByProveedor
{
    public class GetComprasByProveedorQueryHandler : IRequestHandler<GetComprasByProveedorQuery, BaseResponse<List<CompraDto>>>
    {
        private readonly ICompraRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetComprasByProveedorQueryHandler> _logger;

        public GetComprasByProveedorQueryHandler(ICompraRepository repository, IMapper mapper, ILogger<GetComprasByProveedorQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<List<CompraDto>>> Handle(GetComprasByProveedorQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var compras = await _repository.GetByProveedorAsync(request.IdProveedor);
                return BaseResponse<List<CompraDto>>.SuccessResponse(_mapper.Map<List<CompraDto>>(compras), "Compras encontradas correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener compras del proveedor");
                return BaseResponse<List<CompraDto>>.FailureResponse("Ocurrió un error al obtener las compras del proveedor.");
            }
        }
    }
}
