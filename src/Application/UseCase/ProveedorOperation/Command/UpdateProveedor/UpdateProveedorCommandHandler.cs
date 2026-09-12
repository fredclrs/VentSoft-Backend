using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.ProveedorOperation.Command.UpdateProveedor
{
    public class UpdateProveedorCommandHandler : IRequestHandler<UpdateProveedorCommand, BaseResponse<ProveedorDto>>
    {
        private readonly IRepository<Proveedor> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateProveedorCommandHandler> _logger;

        public UpdateProveedorCommandHandler(IRepository<Proveedor> repository, IMapper mapper, ILogger<UpdateProveedorCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<ProveedorDto>> Handle(UpdateProveedorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var proveedor = await _repository.GetByIdAsync(request.Id);
                if (proveedor == null)
                    return BaseResponse<ProveedorDto>.FailureResponse("Proveedor no encontrado.");

                _mapper.Map(request.ProveedorDto, proveedor);
                proveedor.UserActualizado = "system";
                proveedor.FechaActualizado = DateTime.Now;

                _repository.Update(proveedor);
                await _repository.SaveChangesAsync();

                return BaseResponse<ProveedorDto>.SuccessResponse(_mapper.Map<ProveedorDto>(proveedor), "Proveedor actualizado correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar proveedor");
                return BaseResponse<ProveedorDto>.FailureResponse("Ocurrió un error al actualizar el proveedor.");
            }
        }
    }
}
