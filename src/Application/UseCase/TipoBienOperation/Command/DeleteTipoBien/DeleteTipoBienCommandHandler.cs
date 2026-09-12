using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.TipoBienOperation.Command.DeleteTipoBien
{
    public class DeleteTipoBienCommandHandler : IRequestHandler<DeleteTipoBienCommand, BaseResponse<TipoBienDto>>
    {
        private readonly IRepository<TipoBien> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteTipoBienCommandHandler> _logger;

        public DeleteTipoBienCommandHandler(IRepository<TipoBien> repository, IMapper mapper, ILogger<DeleteTipoBienCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<TipoBienDto>> Handle(DeleteTipoBienCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var tipoBien = await _repository.GetByIdAsync(request.Id);
                if (tipoBien == null)
                    return BaseResponse<TipoBienDto>.FailureResponse("No se encontró el tipo de bien.");

                tipoBien.Estado = "IN";
                tipoBien.UserBaja = "system";
                tipoBien.FechaBaja = DateTime.Now;

                _repository.Update(tipoBien);
                await _repository.SaveChangesAsync();

                return BaseResponse<TipoBienDto>.SuccessResponse(_mapper.Map<TipoBienDto>(tipoBien), "Se dio de baja el tipo de bien correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al dar de baja tipo de bien");
                return BaseResponse<TipoBienDto>.FailureResponse("Ocurrió un error al dar de baja el tipo de bien.");
            }
        }
    }
}
