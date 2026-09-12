using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.TipoBienOperation.Queries.GetTipoBienById
{
    public class GetTipoBienByIdQueryHandler : IRequestHandler<GetTipoBienByIdQuery, BaseResponse<TipoBienDto>>
    {
        private readonly IRepository<TipoBien> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetTipoBienByIdQueryHandler> _logger;

        public GetTipoBienByIdQueryHandler(IRepository<TipoBien> repository, IMapper mapper, ILogger<GetTipoBienByIdQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<TipoBienDto>> Handle(GetTipoBienByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var tipoBien = await _repository.GetByIdAsync(request.Id);
                if (tipoBien == null)
                    return BaseResponse<TipoBienDto>.FailureResponse("No se encontró el tipo de bien.");

                return BaseResponse<TipoBienDto>.SuccessResponse(_mapper.Map<TipoBienDto>(tipoBien), "Se encontró el tipo de bien correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener tipo de bien");
                return BaseResponse<TipoBienDto>.FailureResponse("Ocurrió un error al obtener el tipo de bien.");
            }
        }
    }
}
