namespace Domain.Common
{
    /// <summary>Lo que necesita una línea leída/interpretada por IA (factura, lista de texto)
    /// para poder clasificarse contra el catálogo — ver IaHelpers.Clasificar en Application.
    /// Implementado por LineaFacturaDto y LineaTextoProductoDto.</summary>
    public interface ILineaClasificable
    {
        string Descripcion { get; }
        string? Talla { get; }
        string? Color { get; }
        bool EsNuevo { get; set; }
        bool EsVarianteNueva { get; set; }
        int? IdArticuloExistente { get; set; }
        string? CodigoExistente { get; set; }
        string? CodigoGrupo { get; set; }
        int? IdFamiliaGrupo { get; set; }
    }
}
