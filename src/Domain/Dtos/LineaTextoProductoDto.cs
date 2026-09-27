using Domain.Common;

namespace Domain.Dtos
{
    /// <summary>Un renglón que la IA interpretó de una lista de texto libre escrita a mano (ej.
    /// "remera polo azul 4"), ya comparado contra el catálogo — igual que LineaFacturaDto, es
    /// solo una sugerencia para revisar y confirmar.</summary>
    public class LineaTextoProductoDto : ILineaClasificable
    {
        public string Descripcion { get; set; } = null!;
        public string? Talla { get; set; }
        public string? Color { get; set; }
        public int Cantidad { get; set; }

        public bool EsNuevo { get; set; }

        /// <summary>true si el producto (mismo código compartido) ya existe pero esta talla/color
        /// puntual no — hace falta completar Talla/Color nomás, el Código/Familia se reusan (ver
        /// CodigoGrupo/IdFamiliaGrupo). Solo puede pasar si el negocio tiene código compartido
        /// activado.</summary>
        public bool EsVarianteNueva { get; set; }

        public int? IdArticuloExistente { get; set; }
        public string? CodigoExistente { get; set; }

        /// <summary>Solo si EsVarianteNueva es true: el código/familia del grupo ya existente.</summary>
        public string? CodigoGrupo { get; set; }
        public int? IdFamiliaGrupo { get; set; }
    }
}
