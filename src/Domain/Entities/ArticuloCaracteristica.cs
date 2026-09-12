namespace Domain.Entities
{
    /// <summary>
    /// Valor de un atributo libre para un artículo (talla, color, material,
    /// lote, principio activo, etc.). Un artículo puede tener cero, uno o
    /// varios; cada rubro de negocio usa las Caracteristicas que le sirven,
    /// sin tocar el esquema.
    /// </summary>
    public class ArticuloCaracteristica
    {
        public int Id { get; set; }

        public int IdArticulo { get; set; }
        public int IdCaracteristica { get; set; }

        public string Valor { get; set; } = null!;

        public Articulo Articulo { get; set; } = null!;
        public Caracteristica Caracteristica { get; set; } = null!;
    }
}
