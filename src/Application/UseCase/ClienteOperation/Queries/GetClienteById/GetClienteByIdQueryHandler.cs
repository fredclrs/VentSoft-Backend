using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.ClienteOperation.Queries.GetClienteById
{
    public class GetClienteByIdQueryHandler : IRequestHandler<GetClienteByIdQuery, BaseResponse<ClienteDto>>
    {
        private readonly IRepository<Cliente> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetClienteByIdQueryHandler> _logger;

        public GetClienteByIdQueryHandler(IRepository<Cliente> repository, IMapper mapper, ILogger<GetClienteByIdQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<ClienteDto>> Handle(GetClienteByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var cliente = await _repository.GetByIdAsync(request.Id);
                if (cliente == null)
                    return BaseResponse<ClienteDto>.FailureResponse("Cliente no encontrado.");

                return BaseResponse<ClienteDto>.SuccessResponse(_mapper.Map<ClienteDto>(cliente), "Cliente encontrado correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el cliente");
                return BaseResponse<ClienteDto>.FailureResponse("Ocurrió un error al obtener el cliente.");
            }
        }
    }
}
