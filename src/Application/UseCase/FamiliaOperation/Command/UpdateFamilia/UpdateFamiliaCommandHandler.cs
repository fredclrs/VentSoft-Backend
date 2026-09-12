using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.FamiliaOperation.Command.UpdateFamilia
{
    public class UpdateFamiliaCommandHandler : IRequestHandler<UpdateFamiliaCommand, BaseResponse<FamiliaDto>>
    {
        private readonly IRepository<Familia> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateFamiliaCommandHandler> _logger;

        public UpdateFamiliaCommandHandler(IRepository<Familia> repository, IMapper mapper, ILogger<UpdateFamiliaCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<FamiliaDto>> Handle(UpdateFamiliaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var familia = await _repository.GetByIdAsync(request.Id);
                if (familia == null)
                    return BaseResponse<FamiliaDto>.FailureResponse("No se encontró la familia.");

                _mapper.Map(request.FamiliaDto, familia);
                familia.UserActualizado = "system";
                familia.FechaActualizado = DateTime.Now;

                _repository.Update(familia);
                await _repository.SaveChangesAsync();

                return BaseResponse<FamiliaDto>.SuccessResponse(_mapper.Map<FamiliaDto>(familia), "Se actualizó la familia correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar familia");
                return BaseResponse<FamiliaDto>.FailureResponse("Ocurrió un error al actualizar la familia.");
            }
        }
    }
}
