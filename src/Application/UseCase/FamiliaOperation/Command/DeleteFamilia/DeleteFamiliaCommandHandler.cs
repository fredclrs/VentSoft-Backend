using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.FamiliaOperation.Command.DeleteFamilia
{
    public class DeleteFamiliaCommandHandler : IRequestHandler<DeleteFamiliaCommand, BaseResponse<FamiliaDto>>
    {
        private readonly IRepository<Familia> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteFamiliaCommandHandler> _logger;

        public DeleteFamiliaCommandHandler(IRepository<Familia> repository, IMapper mapper, ILogger<DeleteFamiliaCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<FamiliaDto>> Handle(DeleteFamiliaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var familia = await _repository.GetByIdAsync(request.Id);
                if (familia == null)
                    return BaseResponse<FamiliaDto>.FailureResponse("No se encontró la familia.");

                familia.Estado = "IN";
                familia.UserBaja = "system";
                familia.FechaBaja = DateTime.Now;

                _repository.Update(familia);
                await _repository.SaveChangesAsync();

                return BaseResponse<FamiliaDto>.SuccessResponse(_mapper.Map<FamiliaDto>(familia), "Se dio de baja la familia correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al dar de baja familia");
                return BaseResponse<FamiliaDto>.FailureResponse("Ocurrió un error al dar de baja la familia.");
            }
        }
    }
}
