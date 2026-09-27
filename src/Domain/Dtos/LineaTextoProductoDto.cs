using Domain.Common;

namespace Domain.Dtos
{
    /// <summary>Un renglón que la IA interpretó de una lista de texto libre escrita a mano (ej.
    /// "remera polo azul 4"), ya comparado contra el catálogo — igual que LineaFacturaDto, es
    /// solo una sugerencia para revisar y confirmar.</summary>
    public class LineaTextoProductoDto : ILineaClasificable
    {
        public string Descripcion { get; set; } = null!;

        /// <summary>Atributos libres para esta línea (talla, color, material, lote, lo que use
        /// este negocio en particular).</summary>
        public List<ArticuloCaracteristicaDto> Caracteristicas { get; set; } = new();
        public int Cantidad { get; set; }

        public bool EsNuevo { get; set; }

        /// <summary>true si el producto (mismo código compartido) ya existe pero esta variante
        /// puntual no — hace falta completar las Caracteristicas nomás, el Código/Familia se
        /// reusan (ver CodigoGrupo/IdFamiliaGrupo). Solo puede pasar si el negocio tiene código
        /// compartido activado.</summary>
        public bool EsVarianteNueva { get; set; }

        public int? IdArticuloExistente { get; set; }
        public string? CodigoExistente { get; set; }

        /// <summary>Solo si EsVarianteNueva es true: el código/familia del grupo ya existente.</summary>
        public string? CodigoGrupo { get; set; }
        public int? IdFamiliaGrupo { get; set; }
    }
}
