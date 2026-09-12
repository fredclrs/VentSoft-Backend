using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.ProveedorOperation.Command.DeleteProveedor
{
    public class DeleteProveedorCommandHandler : IRequestHandler<DeleteProveedorCommand, BaseResponse<ProveedorDto>>
    {
        private readonly IRepository<Proveedor> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteProveedorCommandHandler> _logger;

        public DeleteProveedorCommandHandler(IRepository<Proveedor> repository, IMapper mapper, ILogger<DeleteProveedorCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<ProveedorDto>> Handle(DeleteProveedorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var proveedor = await _repository.GetByIdAsync(request.Id);
                if (proveedor == null)
                    return BaseResponse<ProveedorDto>.FailureResponse("Proveedor no encontrado.");

                proveedor.Estado = "IN";
                proveedor.UserBaja = "system";
                proveedor.FechaBaja = DateTime.Now;

                _repository.Update(proveedor);
                await _repository.SaveChangesAsync();

                return BaseResponse<ProveedorDto>.SuccessResponse(_mapper.Map<ProveedorDto>(proveedor), "Proveedor dado de baja correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al dar de baja al proveedor");
                return BaseResponse<ProveedorDto>.FailureResponse("Ocurrió un error al dar de baja al proveedor.");
            }
        }
    }
}
