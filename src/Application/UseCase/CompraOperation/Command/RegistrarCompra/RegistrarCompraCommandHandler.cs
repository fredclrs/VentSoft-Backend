using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.CompraOperation.Command.RegistrarCompra
{
    /// <summary>
    /// Registra una Compra junto con todos sus renglones (DetalleCompra) en una sola operación,
    /// y actualiza el costo de cada Articulo comprado al ÚLTIMO costo de compra (no un promedio):
    /// cada compra nueva pisa directo el costo anterior, así el costo del artículo siempre
    /// refleja lo último que se pagó por él, sin mezclarse con compras viejas a otro precio.
    ///
    /// Si el artículo tiene un Margen de ganancia configurado, de paso se calcula un precio de
    /// venta SUGERIDO (Costo / (1 - Margen/100), margen sobre precio de venta — la convención
    /// de indumentaria, no markup sobre costo) — pero no se aplica todavía: viaja en
    /// CompraDto.PreciosSugeridos para que el frontend se lo muestre al cajero después de
    /// guardar, y recién ahí confirme o rechace cada cambio puntual (ver
    /// ActualizarPrecioArticuloCommand, que es el que realmente lo aplica).
    /// </summary>
    public class RegistrarCompraCommandHandler : IRequestHandler<RegistrarCompraCommand, BaseResponse<CompraDto>>
    {
        private readonly ICompraRepository _compraRepository;
        private readonly IArticuloRepository _articuloRepository;
        private readonly IRepository<ConfiguracionEmpresa> _configuracionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RegistrarCompraCommandHandler> _logger;

        public RegistrarCompraCommandHandler(
            ICompraRepository compraRepository,
            IArticuloRepository articuloRepository,
            IRepository<ConfiguracionEmpresa> configuracionRepository,
            IMapper mapper,
            ILogger<RegistrarCompraCommandHandler> logger)
        {
            _compraRepository = compraRepository;
            _articuloRepository = articuloRepository;
            _configuracionRepository = configuracionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<CompraDto>> Handle(RegistrarCompraCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dto = request.CompraDto;

                var compra = new Compra
                {
                    Fecha = dto.Fecha == default ? DateTime.Now : dto.Fecha,
                    Referencias = dto.Referencias,
                    Nota = dto.Nota,
                    IdUsuario = dto.IdUsuario,
                    IdProveedor = dto.IdProveedor,
                    Estado = "AC",
                    FechaRegistro = DateTime.Now,
                    UserRegistro = "system" // TODO: usuario autenticado real
                };

                // Se consulta una sola vez antes del loop (no por artículo) para no repetir la
                // misma consulta N veces en una compra con varios renglones.
                var redondearEnteros = (await _configuracionRepository.GetAllAsync())
                    .FirstOrDefault()?.RedondearPreciosEnteros ?? false;

                double total = 0;
                var preciosSugeridos = new List<PrecioSugeridoDto>();
                var avisosSinMargen = new List<AvisoSinMargenDto>();

                foreach (var d in dto.Detalles)
                {
                    var articulo = await _articuloRepository.GetByIdAsync(d.IdArticulo);
                    if (articulo == null || articulo.Estado != "AC")
                        return BaseResponse<CompraDto>.FailureResponse($"El artículo con Id {d.IdArticulo} no existe o está inactivo.");

                    var subTotal = d.CostoUnitario * d.Cantidad;
                    total += (double)subTotal;

                    compra.Detalles.Add(new DetalleCompra
                    {
                        IdArticulo = d.IdArticulo,
                        Cantidad = d.Cantidad,
                        CostoUnitario = d.CostoUnitario,
                        SubTotal = subTotal,
                        Pagado = 0,
                        Lote = d.Lote,
                        FechaVencimiento = d.FechaVencimiento
                    });

                    // Último costo de compra: pisa directo el costo anterior del artículo (no se
                    // promedia con el stock viejo). Si la misma compra trae varios renglones del
                    // mismo artículo (p.ej. distintos lotes), el que se procesa último es el que
                    // queda — coherente con "el costo es lo último que pagué".
                    //
                    // Articulo.Costo es SIEMPRE "por caja completa" (igual que Precio), pero acá
                    // d.CostoUnitario ya viene convertido a costo POR UNIDAD real (el frontend
                    // hace esa conversión antes de mandarlo, sea que la compra se haya cargado
                    // "por caja" o "por unidad suelta" — el backend siempre trabaja en unidades).
                    // Por eso hay que multiplicar de vuelta por Fraccion para volver a "por caja".
                    // En artículos que no se venden por caja (Fraccion = 1, la inmensa mayoría),
                    // esta multiplicación no cambia nada.
                    var fraccion = articulo.Fraccion > 0 ? articulo.Fraccion : 1;
                    var costoAnterior = articulo.Costo;
                    articulo.Costo = (double)d.CostoUnitario * fraccion;

                    // Sin Margen configurado, el sistema no puede recalcular el precio de venta
                    // solo — pero si el costo subió, avisa igual para que el cajero se acuerde de
                    // revisarlo a mano (ver PrecioSugeridoDto para el caso CON margen). No se
                    // avisa en la primera compra de un artículo nuevo (costoAnterior = 0) ni
                    // cuando el costo bajó o quedó igual.
                    if (!articulo.MargenGanancia.HasValue && costoAnterior > 0 && articulo.Costo > costoAnterior)
                    {
                        avisosSinMargen.Add(new AvisoSinMargenDto
                        {
                            IdArticulo = articulo.Id,
                            Codigo = articulo.Codigo,
                            PrecioActual = articulo.Precio,
                            CostoAnterior = costoAnterior,
                            CostoNuevo = articulo.Costo
                        });
                    }

                    // Margen de ganancia opcional: si está configurado, se calcula el precio de
                    // venta SUGERIDO con el costo recién actualizado — pero no se aplica acá. El
                    // cajero lo confirma o lo rechaza después de guardar (ver
                    // ActualizarPrecioArticuloCommand). Redondeo según la configuración del
                    // negocio: a 2 decimales normalmente, o al entero de ARRIBA (nunca abajo, para
                    // no perder margen) si el negocio no maneja centavos.
                    //
                    // Margen sobre PRECIO DE VENTA, no sobre costo (Costo / (1 - Margen/100)):
                    // es la convención de indumentaria — un margen de 40% significa que el costo
                    // es el 60% del precio final, no que el precio es el costo + 40%. El
                    // validador (ArticuloDtoValidator) exige Margen < 100 para que esto nunca
                    // divida por cero o negativo.
                    //
                    // Solo tiene sentido sugerir un precio nuevo si el costo REALMENTE cambió
                    // (costoAnterior > 0 y distinto del nuevo) — si no, el negocio puede haber
                    // redondeado el precio a mano a un valor "cerrado" que no coincide exacto con
                    // la cuenta del margen, y sin este chequeo se le sugeriría "corregirlo" en
                    // cada compra siguiente aunque compre siempre al mismo costo.
                    if (articulo.MargenGanancia.HasValue && costoAnterior > 0 && articulo.Costo != costoAnterior)
                    {
                        var precioCalculado = articulo.Costo / (1 - articulo.MargenGanancia.Value / 100.0);
                        var precioSugerido = redondearEnteros
                            ? Math.Ceiling(precioCalculado)
                            : Math.Round(precioCalculado, 2);
                        if (precioSugerido != articulo.Precio)
                        {
                            preciosSugeridos.Add(new PrecioSugeridoDto
                            {
                                IdArticulo = articulo.Id,
                                Codigo = articulo.Codigo,
                                PrecioActual = articulo.Precio,
                                PrecioSugerido = precioSugerido
                            });
                        }
                    }

                    articulo.UserActualizado = "system";
                    articulo.FechaActualizado = DateTime.Now;
                    _articuloRepository.Update(articulo);
                }

                compra.Total = total;
                compra.Pagado = dto.Pagado;
                compra.PorPagar = total - dto.Pagado;

                await _compraRepository.AddAsync(compra);
                // Una sola llamada a SaveChangesAsync: persiste la Compra + Detalles y los Articulo
                // actualizados dentro de la misma transacción implícita de EF Core.
                await _compraRepository.SaveChangesAsync();

                var creada = await _compraRepository.GetByIdWithDetallesAsync(compra.Id);
                var resultado = _mapper.Map<CompraDto>(creada);
                resultado.PreciosSugeridos = preciosSugeridos;
                resultado.AvisosSinMargen = avisosSinMargen;
                return BaseResponse<CompraDto>.SuccessResponse(resultado, "Compra registrada correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar la compra");
                return BaseResponse<CompraDto>.FailureResponse("Ocurrió un error al registrar la compra.");
            }
        }
    }
}
