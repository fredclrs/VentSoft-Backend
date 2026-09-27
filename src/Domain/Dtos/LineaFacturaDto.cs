using Domain.Common;

namespace Domain.Dtos
{
    /// <summary>Un renglón que la IA leyó de la foto de una factura de compra, ya comparado
    /// contra el catálogo — la persona lo revisa/corrige antes de confirmar, nunca se crea ni se
    /// agrega nada solo.</summary>
    public class LineaFacturaDto : ILineaClasificable
    {
        public string Descripcion { get; set; } = null!;

        /// <summary>Solo si la factura la menciona por separado — no siempre viene.</summary>
        public string? Talla { get; set; }
        public string? Color { get; set; }

        public int Cantidad { get; set; }
        public double CostoUnitario { get; set; }

        /// <summary>true si no se encontró ningún producto parecido en el catálogo — hace falta
        /// completar Código/Familia/Tamaño para poder darlo de alta como producto nuevo.</summary>
        public bool EsNuevo { get; set; }

        /// <summary>true si el producto (mismo código compartido) ya existe pero esta talla/color
        /// puntual no — hace falta completar el Tamaño nomás, el Código/Familia se reusan
        /// (ver CodigoGrupo/IdFamiliaGrupo). Solo puede pasar si el negocio tiene código
        /// compartido activado.</summary>
        public bool EsVarianteNueva { get; set; }

        /// <summary>Solo si EsNuevo y EsVarianteNueva son false: el artículo del catálogo que
        /// coincidió exactamente (mismo producto Y misma talla/color).</summary>
        public int? IdArticuloExistente { get; set; }
        public string? CodigoExistente { get; set; }

        /// <summary>Solo si EsVarianteNueva es true: el código/familia del grupo ya existente, para
        /// precargar el alta de la variante nueva sin tener que inventarlos.</summary>
        public string? CodigoGrupo { get; set; }
        public int? IdFamiliaGrupo { get; set; }
    }
}
