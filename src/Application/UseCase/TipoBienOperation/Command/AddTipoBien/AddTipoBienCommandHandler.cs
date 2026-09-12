using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.TipoBienOperation.Command.AddTipoBien
{
    public class AddTipoBienCommandHandler : IRequestHandler<AddTipoBienCommand, BaseResponse<TipoBienDto>>
    {
        private readonly IRepository<TipoBien> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<AddTipoBienCommandHandler> _logger;

        public AddTipoBienCommandHandler(IRepository<TipoBien> repository, IMapper mapper, ILogger<AddTipoBienCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<TipoBienDto>> Handle(AddTipoBienCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var tipoBien = _mapper.Map<TipoBien>(request.TipoBienDto);
                tipoBien.Estado = "AC";
                tipoBien.FechaRegistro = DateTime.Now;

                await _repository.AddAsync(tipoBien);
                await _repository.SaveChangesAsync();

                return BaseResponse<TipoBienDto>.SuccessResponse(_mapper.Map<TipoBienDto>(tipoBien), "Se agregó el tipo de bien correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar tipo de bien");
                return BaseResponse<TipoBienDto>.FailureResponse("Ocurrió un error al agregar el tipo de bien.");
            }
        }
    }
}
