using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.ClienteOperation.Command.AddCliente
{
    public class AddClienteCommandHandler : IRequestHandler<AddClienteCommand, BaseResponse<ClienteDto>>
    {
        private readonly IRepository<Cliente> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<AddClienteCommandHandler> _logger;

        public AddClienteCommandHandler(IRepository<Cliente> repository, IMapper mapper, ILogger<AddClienteCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<ClienteDto>> Handle(AddClienteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var cliente = _mapper.Map<Cliente>(request.ClienteDto);
                cliente.Estado = "AC";
                cliente.FechaRegistro = DateTime.Now;

                await _repository.AddAsync(cliente);
                await _repository.SaveChangesAsync();

                return BaseResponse<ClienteDto>.SuccessResponse(_mapper.Map<ClienteDto>(cliente), "Cliente agregado correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar cliente");
                return BaseResponse<ClienteDto>.FailureResponse("Ocurrió un error al agregar el cliente.");
            }
        }
    }
}
