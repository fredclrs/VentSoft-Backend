using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.FamiliaOperation.Command.AddFamilia
{
    public class AddFamiliaCommandHandler : IRequestHandler<AddFamiliaCommand, BaseResponse<FamiliaDto>>
    {
        private readonly IRepository<Familia> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<AddFamiliaCommandHandler> _logger;

        public AddFamiliaCommandHandler(IRepository<Familia> repository, IMapper mapper, ILogger<AddFamiliaCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<FamiliaDto>> Handle(AddFamiliaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var familia = _mapper.Map<Familia>(request.FamiliaDto);
                familia.Estado = "AC";
                familia.FechaRegistro = DateTime.Now;

                await _repository.AddAsync(familia);
                await _repository.SaveChangesAsync();

                return BaseResponse<FamiliaDto>.SuccessResponse(_mapper.Map<FamiliaDto>(familia), "Se agregó la familia correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar familia");
                return BaseResponse<FamiliaDto>.FailureResponse("Ocurrió un error al agregar la familia.");
            }
        }
    }
}
