using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.ProveedorOperation.Queries.SearchProveedores
{
    public class SearchProveedorQueryHandler : IRequestHandler<SearchProveedorQuery, BaseResponse<List<ProveedorDto>>>
    {
        private readonly IRepository<Proveedor> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SearchProveedorQueryHandler> _logger;

        public SearchProveedorQueryHandler(IRepository<Proveedor> repository, IMapper mapper, ILogger<SearchProveedorQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<List<ProveedorDto>>> Handle(SearchProveedorQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // A propósito NO se filtra por Estado acá: "Compras por proveedor" usa esta
                // misma búsqueda para armar un mapa id→proveedor y mostrar el nombre real en
                // compras viejas, aunque el proveedor ya esté dado de baja. El filtro de "solo
                // activos" para la pantalla de gestión de Proveedores se hace del lado del
                // frontend (ProveedoresPage), no acá.
                var proveedores = (await _repository.GetAllAsync()).AsEnumerable();

                // Nombre y Nit se combinan con OR (no AND): así el buscador de la pantalla puede
                // mandar el mismo texto tipeado en los dos campos y encontrar el proveedor sin
                // importar si coincide por nombre o por NIT (útil para distinguir dos proveedores
                // con el mismo nombre pero distinto NIT).
                if (!string.IsNullOrWhiteSpace(request.Nombre) || !string.IsNullOrWhiteSpace(request.Nit))
                {
                    proveedores = proveedores.Where(p =>
                        (!string.IsNullOrWhiteSpace(request.Nombre) && p.Nombre.Contains(request.Nombre, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrWhiteSpace(request.Nit) && p.Nit != null && p.Nit.Contains(request.Nit, StringComparison.OrdinalIgnoreCase)));
                }

                var resultado = _mapper.Map<List<ProveedorDto>>(proveedores.ToList());

                return BaseResponse<List<ProveedorDto>>.SuccessResponse(resultado, "Proveedores encontrados correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar proveedores");
                return BaseResponse<List<ProveedorDto>>.FailureResponse("Ocurrió un error al buscar proveedores.");
            }
        }
    }
}
