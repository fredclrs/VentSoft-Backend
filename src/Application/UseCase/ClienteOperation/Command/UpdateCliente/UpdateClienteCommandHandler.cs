using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.ClienteOperation.Command.UpdateCliente
{
    public class UpdateClienteCommandHandler : IRequestHandler<UpdateClienteCommand, BaseResponse<ClienteDto>>
    {
        private readonly IRepository<Cliente> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateClienteCommandHandler> _logger;

        public UpdateClienteCommandHandler(IRepository<Cliente> repository, IMapper mapper, ILogger<UpdateClienteCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<ClienteDto>> Handle(UpdateClienteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var cliente = await _repository.GetByIdAsync(request.Id);
                if (cliente == null)
                    return BaseResponse<ClienteDto>.FailureResponse("Cliente no encontrado.");

                _mapper.Map(request.ClienteDto, cliente);
                cliente.UserActualizado = "system"; // TODO: usuario autenticado real
                cliente.FechaActualizado = DateTime.Now;

                _repository.Update(cliente);
                await _repository.SaveChangesAsync();

                return BaseResponse<ClienteDto>.SuccessResponse(_mapper.Map<ClienteDto>(cliente), "Cliente actualizado correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar cliente");
                return BaseResponse<ClienteDto>.FailureResponse("Ocurrió un error al actualizar el cliente.");
            }
        }
    }
}
