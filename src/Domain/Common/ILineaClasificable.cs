using Domain.Dtos;

namespace Domain.Common
{
    /// <summary>Lo que necesita una línea leída/interpretada por IA (factura, lista de texto)
    /// para poder clasificarse contra el catálogo — ver IaHelpers.Clasificar en Application.
    /// Implementado por LineaFacturaDto y LineaTextoProductoDto.</summary>
    public interface ILineaClasificable
    {
        string Descripcion { get; }

        /// <summary>Atributos libres leídos para esta línea (talla/color para indumentaria,
        /// material/lote/lo que use cada rubro — según la Caracteristica que tenga configurada
        /// el negocio). El Tamaño de la variante se arma uniendo estos Valor, igual que en el
        /// alta manual ("+ Variante").</summary>
        List<ArticuloCaracteristicaDto> Caracteristicas { get; }
        bool EsNuevo { get; set; }
        bool EsVarianteNueva { get; set; }
        int? IdArticuloExistente { get; set; }
        string? CodigoExistente { get; set; }
        string? CodigoGrupo { get; set; }
        int? IdFamiliaGrupo { get; set; }
    }
}
