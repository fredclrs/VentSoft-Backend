using Application;
using Application.UseCase.ArticuloOperation.Command.ActualizarPrecioArticulo;
using Application.UseCase.ArticuloOperation.Command.AddArticulo;
using Application.UseCase.ArticuloOperation.Command.DeleteArticulo;
using Application.UseCase.ArticuloOperation.Command.UpdateArticulo;
using Application.UseCase.ArticuloOperation.Queries.GetArticuloById;
using Application.UseCase.ArticuloOperation.Queries.GetArticulosStockBajo;
using Application.UseCase.ArticuloOperation.Queries.GetStockArticulo;
using Application.UseCase.ArticuloOperation.Queries.GetStockTodos;
using Application.UseCase.ArticuloOperation.Queries.SearchArticulos;
using Domain.Common;
using Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebVentSoft.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticuloController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ArticuloController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>Crea un nuevo artículo, con sus atributos (talla, color, lote, etc.) si los tiene.</summary>
        [HttpPost]
        [Authorize(Roles = Permisos.Inventario)]
        [ProducesResponseType(typeof(BaseResponse<ArticuloDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<ArticuloDto>), 400)]
        public async Task<IActionResult> Insert([FromBody] ArticuloDto articuloDto)
        {
            var response = await _mediator.Send(new AddArticuloCommand { ArticuloDto = articuloDto });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Actualiza un artículo (reemplaza también el set completo de atributos enviado).</summary>
        [HttpPut]
        [Authorize(Roles = Permisos.Inventario)]
        [ProducesResponseType(typeof(BaseResponse<ArticuloDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<ArticuloDto>), 400)]
        public async Task<IActionResult> Update([FromQuery] int id, [FromBody] ArticuloDto articuloDto)
        {
            var response = await _mediator.Send(new UpdateArticuloCommand { Id = id, ArticuloDto = articuloDto });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Da de baja (lógica) a un artículo.</summary>
        [HttpDelete]
        [Authorize(Roles = Permisos.Inventario)]
        [ProducesResponseType(typeof(BaseResponse<ArticuloDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<ArticuloDto>), 400)]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var response = await _mediator.Send(new DeleteArticuloCommand { Id = id });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        public class ActualizarPrecioRequest
        {
            public double Precio { get; set; }
        }

        /// <summary>Aplica un precio sugerido por margen de ganancia (confirmado por el cajero
        /// después de una Compra) — toca únicamente Precio. Mismo permiso que Compras: quien
        /// registra la compra que generó la sugerencia es quien la confirma o rechaza.</summary>
        [HttpPut("precio")]
        [Authorize(Roles = Permisos.Compras)]
        [ProducesResponseType(typeof(BaseResponse<ArticuloDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<ArticuloDto>), 400)]
        public async Task<IActionResult> ActualizarPrecio([FromQuery] int idArticulo, [FromBody] ActualizarPrecioRequest request)
        {
            var response = await _mediator.Send(new ActualizarPrecioArticuloCommand { IdArticulo = idArticulo, Precio = request.Precio });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Devuelve un artículo (con sus atributos) por Id.</summary>
        [HttpGet("byId")]
        [ProducesResponseType(typeof(BaseResponse<ArticuloDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<ArticuloDto>), 400)]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var response = await _mediator.Send(new GetArticuloByIdQuery { Id = id });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Busca artículos por código, descripción y/o familia (categoría).</summary>
        [HttpGet("search")]
        [ProducesResponseType(typeof(BaseResponse<List<ArticuloDto>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<List<ArticuloDto>>), 400)]
        public async Task<IActionResult> Search([FromQuery] string? codigo = null, [FromQuery] string? descripcion = null, [FromQuery] int? idFamilia = null)
        {
            var response = await _mediator.Send(new SearchArticuloQuery { Codigo = codigo, Descripcion = descripcion, IdFamilia = idFamilia });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Stock actual de un artículo (compras - ventas registradas).</summary>
        [HttpGet("stock")]
        [ProducesResponseType(typeof(BaseResponse<ArticuloStockDto>), 200)]
        [ProducesResponseType(typeof(BaseResponse<ArticuloStockDto>), 400)]
        public async Task<IActionResult> GetStock([FromQuery] int idArticulo)
        {
            var response = await _mediator.Send(new GetStockArticuloQuery { IdArticulo = idArticulo });
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Artículos activos cuyo stock actual llegó o bajó de su StockMinimo.</summary>
        [HttpGet("stockBajo")]
        [ProducesResponseType(typeof(BaseResponse<List<ArticuloStockDto>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<List<ArticuloStockDto>>), 400)]
        public async Task<IActionResult> GetStockBajo()
        {
            var response = await _mediator.Send(new GetArticulosStockBajoQuery());
            return response.Success ? Ok(response) : BadRequest(response);
        }

        /// <summary>Stock actual de todos los artículos activos, de una sola vez — para pantallas
        /// como Ventas que necesitan saber qué artículos no tienen stock antes de dejarlos elegir.</summary>
        [HttpGet("stockTodos")]
        [ProducesResponseType(typeof(BaseResponse<List<ArticuloStockDto>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<List<ArticuloStockDto>>), 400)]
        public async Task<IActionResult> GetStockTodos()
        {
            var response = await _mediator.Send(new GetStockTodosQuery());
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
