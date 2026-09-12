using Application.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.ConfiguracionEmpresaOperation.Command.UpdateConfiguracionEmpresa
{
    public class UpdateConfiguracionEmpresaCommandHandler : IRequestHandler<UpdateConfiguracionEmpresaCommand, BaseResponse<ConfiguracionEmpresaDto>>
    {
        private readonly IRepository<ConfiguracionEmpresa> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateConfiguracionEmpresaCommandHandler> _logger;

        public UpdateConfiguracionEmpresaCommandHandler(IRepository<ConfiguracionEmpresa> repository, IMapper mapper, ILogger<UpdateConfiguracionEmpresaCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<ConfiguracionEmpresaDto>> Handle(UpdateConfiguracionEmpresaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var configuracion = (await _repository.GetAllAsync()).FirstOrDefault();

                if (configuracion == null)
                {
                    configuracion = new ConfiguracionEmpresa();
                    await _repository.AddAsync(configuracion);
                }

                configuracion.Nombre = request.Nombre;
                configuracion.Moneda = request.Moneda;
                configuracion.PermiteVentaACredito = request.PermiteVentaACredito;
                configuracion.PermiteCompraACredito = request.PermiteCompraACredito;
                configuracion.IdClientePorDefecto = request.IdClientePorDefecto;
                configuracion.IdProveedorPorDefecto = request.IdProveedorPorDefecto;

                if (configuracion.Id != 0)
                    _repository.Update(configuracion);

                await _repository.SaveChangesAsync();

                return BaseResponse<ConfiguracionEmpresaDto>.SuccessResponse(_mapper.Map<ConfiguracionEmpresaDto>(configuracion), "Se actualizó la configuración del negocio correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la configuración del negocio");
                return BaseResponse<ConfiguracionEmpresaDto>.FailureResponse("Ocurrió un error al actualizar la configuración del negocio.");
            }
        }
    }
}
