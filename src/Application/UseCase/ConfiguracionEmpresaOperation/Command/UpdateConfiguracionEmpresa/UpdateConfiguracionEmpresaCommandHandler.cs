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
        private readonly ICifradoService _cifradoService;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateConfiguracionEmpresaCommandHandler> _logger;

        public UpdateConfiguracionEmpresaCommandHandler(
            IRepository<ConfiguracionEmpresa> repository,
            ICifradoService cifradoService,
            IMapper mapper,
            ILogger<UpdateConfiguracionEmpresaCommandHandler> logger)
        {
            _repository = repository;
            _cifradoService = cifradoService;
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
                configuracion.RedondearPreciosEnteros = request.RedondearPreciosEnteros;
                configuracion.PermiteCodigoCompartidoEntreArticulos = request.PermiteCodigoCompartidoEntreArticulos;
                configuracion.IdClientePorDefecto = request.IdClientePorDefecto;
                configuracion.IdProveedorPorDefecto = request.IdProveedorPorDefecto;

                // Solo se toca si mandaron una clave nueva de verdad — el frontend nunca conoce
                // la clave ya guardada (no se la devolvemos), así que si no la reenvía es porque
                // no la está cambiando, no porque quiera borrarla (para eso está el flag de abajo).
                if (request.EliminarClaveApiIA)
                    configuracion.ClaveApiIACifrada = null;
                else if (!string.IsNullOrWhiteSpace(request.ClaveApiIA))
                    configuracion.ClaveApiIACifrada = _cifradoService.Cifrar(request.ClaveApiIA);

                if (configuracion.Id != 0)
                    _repository.Update(configuracion);

                await _repository.SaveChangesAsync();

                var dto = _mapper.Map<ConfiguracionEmpresaDto>(configuracion);
                dto.TieneClaveApiIA = configuracion.ClaveApiIACifrada != null;
                return BaseResponse<ConfiguracionEmpresaDto>.SuccessResponse(dto, "Se actualizó la configuración del negocio correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la configuración del negocio");
                return BaseResponse<ConfiguracionEmpresaDto>.FailureResponse("Ocurrió un error al actualizar la configuración del negocio.");
            }
        }
    }
}
