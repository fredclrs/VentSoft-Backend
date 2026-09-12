using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.ConfiguracionEmpresaOperation.Queries.GetConfiguracionEmpresa
{
    public class GetConfiguracionEmpresaQueryHandler : IRequestHandler<GetConfiguracionEmpresaQuery, BaseResponse<ConfiguracionEmpresaDto>>
    {
        private readonly IRepository<ConfiguracionEmpresa> _repository;
        private readonly IRepository<Cliente> _clienteRepository;
        private readonly IRepository<Proveedor> _proveedorRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetConfiguracionEmpresaQueryHandler> _logger;

        public GetConfiguracionEmpresaQueryHandler(
            IRepository<ConfiguracionEmpresa> repository,
            IRepository<Cliente> clienteRepository,
            IRepository<Proveedor> proveedorRepository,
            IMapper mapper,
            ILogger<GetConfiguracionEmpresaQueryHandler> logger)
        {
            _repository = repository;
            _clienteRepository = clienteRepository;
            _proveedorRepository = proveedorRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<ConfiguracionEmpresaDto>> Handle(GetConfiguracionEmpresaQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var configuracion = (await _repository.GetAllAsync()).FirstOrDefault();

                if (configuracion == null)
                {
                    configuracion = new ConfiguracionEmpresa { Nombre = "VentSoft" };
                    await _repository.AddAsync(configuracion);
                    await _repository.SaveChangesAsync();
                }

                var dto = _mapper.Map<ConfiguracionEmpresaDto>(configuracion);

                // Se resuelven acá (y no con un Include, porque el repositorio genérico no lo
                // soporta) para que el frontend pueda preseleccionar el cliente/proveedor por
                // defecto sin una consulta extra al entrar a Ventas/Compras.
                if (configuracion.IdClientePorDefecto.HasValue)
                {
                    var cliente = await _clienteRepository.GetByIdAsync(configuracion.IdClientePorDefecto.Value);
                    if (cliente != null && cliente.Estado == "AC")
                        dto.ClientePorDefecto = _mapper.Map<ClienteDto>(cliente);
                }

                if (configuracion.IdProveedorPorDefecto.HasValue)
                {
                    var proveedor = await _proveedorRepository.GetByIdAsync(configuracion.IdProveedorPorDefecto.Value);
                    if (proveedor != null && proveedor.Estado == "AC")
                        dto.ProveedorPorDefecto = _mapper.Map<ProveedorDto>(proveedor);
                }

                return BaseResponse<ConfiguracionEmpresaDto>.SuccessResponse(dto, "Configuración obtenida correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la configuración del negocio");
                return BaseResponse<ConfiguracionEmpresaDto>.FailureResponse("Ocurrió un error al obtener la configuración del negocio.");
            }
        }
    }
}
