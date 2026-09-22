using Application;
using Application.UseCase.EtiquetaPendienteOperation.Command.ActualizarCantidadEtiquetaPendiente;
using Application.UseCase.EtiquetaPendienteOperation.Command.AgregarEtiquetaPendiente;
using Application.UseCase.EtiquetaPendienteOperation.Command.EliminarEtiquetaPendiente;
using Application.UseCase.EtiquetaPendienteOperation.Command.VaciarEtiquetasPendientes;
using Application.UseCase.EtiquetaPendienteOperation.Queries.GetEtiquetasPendientes;
using Domain.Common;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebVentSoft.API.Controllers
{
    /// <summary>Cola de etiquetas pendientes de imprimir — para juntar varios productos nuevos
    /// (a veces pocas unidades de cada uno) y mandarlos todos a una sola impresión en vez de
    /// desperdiciar una hoja por cada uno. Mismo permiso que el resto del catálogo de inventario.</summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = Permisos.Inventario)]
    public class EtiquetaPendienteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EtiquetaPendienteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<EtiquetaPendienteDto>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<List<EtiquetaPendienteDto>>), 400)]
        public async Task<IActionResult> GetAll()
        {
            var response = await _mediator.Send(new GetEtiquetasPendientesQuery());
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<EtiquetaPendienteDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<EtiquetaPendienteDto>), 400)]
        public async Task<IActionResult> Agregar([FromBody] AgregarEtiquetaPendienteDto etiquetaDto)
        {
            var response = await _mediator.Send(new AgregarEtiquetaPendienteCommand { EtiquetaDto = etiquetaDto });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Corrige la cantidad de una fila ya cargada en la cola.</summary>
        [HttpPut]
        [ProducesResponseType(typeof(BaseResponse<EtiquetaPendienteDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<EtiquetaPendienteDto>), 400)]
        public async Task<IActionResult> ActualizarCantidad([FromQuery] int id, [FromQuery] int cantidad)
        {
            var response = await _mediator.Send(new ActualizarCantidadEtiquetaPendienteCommand { Id = id, Cantidad = cantidad });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(BaseResponse<bool>), 200)]
        [ProducesResponseType(typeof(BaseResponse<bool>), 400)]
        public async Task<IActionResult> Eliminar([FromQuery] int id)
        {
            var response = await _mediator.Send(new EliminarEtiquetaPendienteCommand { Id = id });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Vacía toda la cola de una vez — se usa después de mandar a imprimir todo.</summary>
        [HttpDelete("vaciar")]
        [ProducesResponseType(typeof(BaseResponse<int>), 200)]
        [ProducesResponseType(typeof(BaseResponse<int>), 400)]
        public async Task<IActionResult> Vaciar()
        {
            var response = await _mediator.Send(new VaciarEtiquetasPendientesCommand());
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
