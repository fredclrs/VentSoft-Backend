using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.ProveedorOperation.Queries.GetProveedorById
{
    public class GetProveedorByIdQueryHandler : IRequestHandler<GetProveedorByIdQuery, BaseResponse<ProveedorDto>>
    {
        private readonly IRepository<Proveedor> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetProveedorByIdQueryHandler> _logger;

        public GetProveedorByIdQueryHandler(IRepository<Proveedor> repository, IMapper mapper, ILogger<GetProveedorByIdQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<ProveedorDto>> Handle(GetProveedorByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var proveedor = await _repository.GetByIdAsync(request.Id);
                if (proveedor == null)
                    return BaseResponse<ProveedorDto>.FailureResponse("Proveedor no encontrado.");

                return BaseResponse<ProveedorDto>.SuccessResponse(_mapper.Map<ProveedorDto>(proveedor), "Proveedor encontrado correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el proveedor");
                return BaseResponse<ProveedorDto>.FailureResponse("Ocurrió un error al obtener el proveedor.");
            }
        }
    }
}
