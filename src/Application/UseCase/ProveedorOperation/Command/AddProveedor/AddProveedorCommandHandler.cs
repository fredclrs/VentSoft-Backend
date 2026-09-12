using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.ProveedorOperation.Command.AddProveedor
{
    public class AddProveedorCommandHandler : IRequestHandler<AddProveedorCommand, BaseResponse<ProveedorDto>>
    {
        private readonly IRepository<Proveedor> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<AddProveedorCommandHandler> _logger;

        public AddProveedorCommandHandler(IRepository<Proveedor> repository, IMapper mapper, ILogger<AddProveedorCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<ProveedorDto>> Handle(AddProveedorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var proveedor = _mapper.Map<Proveedor>(request.ProveedorDto);
                proveedor.Estado = "AC";
                proveedor.FechaRegistro = DateTime.Now;

                await _repository.AddAsync(proveedor);
                await _repository.SaveChangesAsync();

                return BaseResponse<ProveedorDto>.SuccessResponse(_mapper.Map<ProveedorDto>(proveedor), "Proveedor agregado correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar proveedor");
                return BaseResponse<ProveedorDto>.FailureResponse("Ocurrió un error al agregar el proveedor.");
            }
        }
    }
}
