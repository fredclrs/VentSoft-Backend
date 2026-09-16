using Domain.Common;

namespace Domain.Entities
{
    public class Articulo : AuditEntity
    {
        public string Codigo { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string Tamano { get; set; } = null!;
        public string? UnidadMedida { get; set; }

        /// <summary>Unidades sueltas que contiene un paquete/caja de este artículo (1 = no se
        /// vende por caja, se vende suelto siempre — es el caso normal). Precio/Costo son el
        /// precio/costo de UN paquete/caja completo; PrecioUnidadSuelta es lo que sale vender
        /// una sola unidad rota de esa caja (normalmente más caro por unidad).</summary>
        public double Fraccion { get; set; }
        public double Precio { get; set; }
        public double Costo { get; set; }

        /// <summary>Precio de una unidad suelta (fuera de la caja). Null = todavía no se cargó;
        /// se usa Precio/Fraccion como valor de referencia mientras tanto. Solo tiene sentido
        /// cuando Fraccion > 1.</summary>
        public double? PrecioUnidadSuelta { get; set; }

        /// <summary>Margen de ganancia deseado sobre el PRECIO DE VENTA (%, ej. 40 = 40%: de
        /// cada $100 que entran, $40 son ganancia), opcional. Así calculan el precio los negocios
        /// de indumentaria (es la convención del rubro, no markup sobre costo). Si está cargado,
        /// Precio se recalcula solo como Costo / (1 - Margen/100) cada vez que cambia el Costo
        /// (al registrar una Compra) — no hace falta tocarlo a mano. Si es null, Precio sigue
        /// siendo 100% manual, como antes de este campo.</summary>
        public double? MargenGanancia { get; set; }
        public double? StockMinimo { get; set; }
        public double? StockIdeal { get; set; }
        public string? Imagen { get; set; }

        public int IdFamilia { get; set; }

        /// <summary>Opcional: no todo artículo tiene una promoción vigente.</summary>
        public int? IdPromocion { get; set; }

        public Familia Familia { get; set; } = null!;
        public Promocion? Promocion { get; set; }

        /// <summary>Atributos libres del artículo (talla, color, lote, etc. según el rubro).</summary>
        public ICollection<ArticuloCaracteristica> Caracteristicas { get; set; } = new List<ArticuloCaracteristica>();

        public ICollection<DetalleCompra> DetalleCompras { get; set; } = new List<DetalleCompra>();
        public ICollection<DetalleVenta> DetalleVentas { get; set; } = new List<DetalleVenta>();
        public ICollection<DetalleDevolucionVenta> DetalleDevolucionesVenta { get; set; } = new List<DetalleDevolucionVenta>();
        public ICollection<DetalleCambioVenta> DetalleCambiosVenta { get; set; } = new List<DetalleCambioVenta>();
    }
}
