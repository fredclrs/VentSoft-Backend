using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.ClienteOperation.Command.DeleteCliente
{
    public class DeleteClienteCommandHandler : IRequestHandler<DeleteClienteCommand, BaseResponse<ClienteDto>>
    {
        private readonly IRepository<Cliente> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteClienteCommandHandler> _logger;

        public DeleteClienteCommandHandler(IRepository<Cliente> repository, IMapper mapper, ILogger<DeleteClienteCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<ClienteDto>> Handle(DeleteClienteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var cliente = await _repository.GetByIdAsync(request.Id);
                if (cliente == null)
                    return BaseResponse<ClienteDto>.FailureResponse("Cliente no encontrado.");

                // Baja lógica: el cliente puede tener Ventas/Cobros asociados.
                cliente.Estado = "IN";
                cliente.UserBaja = "system"; // TODO: usuario autenticado real
                cliente.FechaBaja = DateTime.Now;

                _repository.Update(cliente);
                await _repository.SaveChangesAsync();

                return BaseResponse<ClienteDto>.SuccessResponse(_mapper.Map<ClienteDto>(cliente), "Cliente dado de baja correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al dar de baja al cliente");
                return BaseResponse<ClienteDto>.FailureResponse("Ocurrió un error al dar de baja al cliente.");
            }
        }
    }
}
