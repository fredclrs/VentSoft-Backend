using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.CobroOperation.Queries.GetCobrosByCliente
{
    public class GetCobrosByClienteQueryHandler : IRequestHandler<GetCobrosByClienteQuery, BaseResponse<List<CobroDto>>>
    {
        private readonly IRepository<Cobro> _cobroRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCobrosByClienteQueryHandler> _logger;

        public GetCobrosByClienteQueryHandler(IRepository<Cobro> cobroRepository, IMapper mapper, ILogger<GetCobrosByClienteQueryHandler> logger)
        {
            _cobroRepository = cobroRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<List<CobroDto>>> Handle(GetCobrosByClienteQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var cobros = (await _cobroRepository.GetAllAsync())
                    .Where(c => c.IdCliente == request.IdCliente)
                    .OrderByDescending(c => c.Fecha)
                    .ToList();

                return BaseResponse<List<CobroDto>>.SuccessResponse(_mapper.Map<List<CobroDto>>(cobros), "Cobros encontrados correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener cobros del cliente");
                return BaseResponse<List<CobroDto>>.FailureResponse("Ocurrió un error al obtener los cobros del cliente.");
            }
        }
    }
}
