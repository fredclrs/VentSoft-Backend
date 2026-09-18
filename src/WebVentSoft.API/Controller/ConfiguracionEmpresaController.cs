using Application;
using Application.UseCase.ConfiguracionEmpresaOperation.Command.UpdateConfiguracionEmpresa;
using Application.UseCase.ConfiguracionEmpresaOperation.Queries.GetConfiguracionEmpresa;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebVentSoft.API.Controllers
{
    /// <summary>
    /// Configuración general del negocio (nombre y moneda) — se muestra en la barra superior,
    /// en todos los montos del frontend y en todos los comprobantes/reportes que se imprimen.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ConfiguracionEmpresaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ConfiguracionEmpresaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>Devuelve la configuración del negocio (la crea con valores por defecto si no existe).</summary>
        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<Domain.Dtos.ConfiguracionEmpresaDto>), 200)]
        public async Task<IActionResult> Get()
        {
            var response = await _mediator.Send(new GetConfiguracionEmpresaQuery());
            return response.Success ? Ok(response) : BadRequest(response);
        }

        public class ActualizarConfiguracionRequest
        {
            public string Nombre { get; set; } = null!;
            public string Moneda { get; set; } = null!;
            public bool PermiteVentaACredito { get; set; } = true;
            public bool PermiteCompraACredito { get; set; } = true;
            public bool RedondearPreciosEnteros { get; set; } = false;
            public bool PermiteCodigoCompartidoEntreArticulos { get; set; } = false;
            public int? IdClientePorDefecto { get; set; }
            public int? IdProveedorPorDefecto { get; set; }
        }

        /// <summary>Actualiza el nombre del negocio, el símbolo de moneda, si vende/compra a
        /// crédito, si redondea precios a enteros, si permite código compartido entre artículos,
        /// y el cliente/proveedor por defecto.</summary>
        [HttpPut]
        [Authorize(Roles = Permisos.Configuracion)]
        [ProducesResponseType(typeof(BaseResponse<Domain.Dtos.ConfiguracionEmpresaDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<Domain.Dtos.ConfiguracionEmpresaDto>), 400)]
        public async Task<IActionResult> Update([FromBody] ActualizarConfiguracionRequest request)
        {
            var response = await _mediator.Send(new UpdateConfiguracionEmpresaCommand
            {
                Nombre = request.Nombre,
                Moneda = request.Moneda,
                PermiteVentaACredito = request.PermiteVentaACredito,
                PermiteCompraACredito = request.PermiteCompraACredito,
                RedondearPreciosEnteros = request.RedondearPreciosEnteros,
                PermiteCodigoCompartidoEntreArticulos = request.PermiteCodigoCompartidoEntreArticulos,
                IdClientePorDefecto = request.IdClientePorDefecto,
                IdProveedorPorDefecto = request.IdProveedorPorDefecto,
            });
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
