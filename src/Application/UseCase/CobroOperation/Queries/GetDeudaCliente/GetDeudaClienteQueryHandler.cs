using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.CobroOperation.Queries.GetDeudaCliente
{
    public class GetDeudaClienteQueryHandler : IRequestHandler<GetDeudaClienteQuery, BaseResponse<ClienteDeudaDto>>
    {
        private readonly IRepository<Cliente> _clienteRepository;
        private readonly IVentaRepository _ventaRepository;
        private readonly ILogger<GetDeudaClienteQueryHandler> _logger;

        public GetDeudaClienteQueryHandler(IRepository<Cliente> clienteRepository, IVentaRepository ventaRepository, ILogger<GetDeudaClienteQueryHandler> logger)
        {
            _clienteRepository = clienteRepository;
            _ventaRepository = ventaRepository;
            _logger = logger;
        }

        public async Task<BaseResponse<ClienteDeudaDto>> Handle(GetDeudaClienteQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var cliente = await _clienteRepository.GetByIdAsync(request.IdCliente);
                if (cliente == null)
                    return BaseResponse<ClienteDeudaDto>.FailureResponse("Cliente no encontrado.");

                // Un cambio con diferencia a favor del negocio ya queda reflejado directamente en
                // Venta.PorPagar (ver RegistrarDevolucionCommandHandler) — no hace falta sumar nada
                // de DevolucionVenta acá, sumarlo aparte contaría la misma deuda dos veces.
                var deuda = (await _ventaRepository.GetByClienteAsync(request.IdCliente))
                    .Where(v => v.Estado == "AC")
                    .Sum(v => v.PorPagar);

                var dto = new ClienteDeudaDto
                {
                    IdCliente = cliente.Id,
                    NombreCliente = cliente.Nombre,
                    DeudaActual = deuda,
                    SaldoAFavor = cliente.SaldoAFavor
                };

                return BaseResponse<ClienteDeudaDto>.SuccessResponse(dto, "Deuda consultada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar la deuda del cliente");
                return BaseResponse<ClienteDeudaDto>.FailureResponse("Ocurrió un error al consultar la deuda del cliente.");
            }
        }
    }
}
