using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.ClienteOperation.Queries.SearchClientes
{
    public class SearchClienteQueryHandler : IRequestHandler<SearchClienteQuery, BaseResponse<List<ClienteDto>>>
    {
        private readonly IRepository<Cliente> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SearchClienteQueryHandler> _logger;

        public SearchClienteQueryHandler(IRepository<Cliente> repository, IMapper mapper, ILogger<SearchClienteQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<List<ClienteDto>>> Handle(SearchClienteQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Tabla chica (negocio pequeño): se filtra en memoria para no acoplar
                // Application a EF. Si la tabla creciera mucho convendría un método
                // de búsqueda específico en el repositorio.
                //
                // A propósito NO se filtra por Estado acá: varios reportes (Ventas del día,
                // Cuentas por cobrar, etc.) usan esta misma búsqueda para armar un mapa
                // id→cliente y mostrar el nombre real en ventas viejas, aunque el cliente ya
                // esté dado de baja. El filtro de "solo activos" para la pantalla de gestión de
                // Clientes se hace del lado del frontend (ClientesPage), no acá.
                var clientes = (await _repository.GetAllAsync()).AsEnumerable();

                // Nombre y DocumentoIdentidad se combinan con OR (no AND): así el buscador de la
                // pantalla puede mandar el mismo texto tipeado en los dos campos y encontrar el
                // cliente sin importar si coincide por nombre o por documento (útil para distinguir
                // dos clientes con el mismo nombre pero distinto documento).
                if (!string.IsNullOrWhiteSpace(request.Nombre) || !string.IsNullOrWhiteSpace(request.DocumentoIdentidad))
                {
                    clientes = clientes.Where(c =>
                        (!string.IsNullOrWhiteSpace(request.Nombre) && c.Nombre.Contains(request.Nombre, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrWhiteSpace(request.DocumentoIdentidad) && c.DocumentoIdentidad.Contains(request.DocumentoIdentidad, StringComparison.OrdinalIgnoreCase)));
                }

                var resultado = _mapper.Map<List<ClienteDto>>(clientes.ToList());

                return BaseResponse<List<ClienteDto>>.SuccessResponse(resultado, "Clientes encontrados correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar clientes");
                return BaseResponse<List<ClienteDto>>.FailureResponse("Ocurrió un error al buscar clientes.");
            }
        }
    }
}
