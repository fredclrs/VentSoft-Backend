using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.CompraOperation.Queries.GetCompraById
{
    public class GetCompraByIdQueryHandler : IRequestHandler<GetCompraByIdQuery, BaseResponse<CompraDto>>
    {
        private readonly ICompraRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCompraByIdQueryHandler> _logger;

        public GetCompraByIdQueryHandler(ICompraRepository repository, IMapper mapper, ILogger<GetCompraByIdQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<CompraDto>> Handle(GetCompraByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var compra = await _repository.GetByIdWithDetallesAsync(request.Id);
                if (compra == null)
                    return BaseResponse<CompraDto>.FailureResponse("Compra no encontrada.");

                return BaseResponse<CompraDto>.SuccessResponse(_mapper.Map<CompraDto>(compra), "Compra encontrada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la compra");
                return BaseResponse<CompraDto>.FailureResponse("Ocurrió un error al obtener la compra.");
            }
        }
    }
}
