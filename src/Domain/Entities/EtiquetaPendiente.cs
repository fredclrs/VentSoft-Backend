namespace Domain.Entities
{
    /// <summary>
    /// Un artículo (y cantidad) esperando a imprimirse — la "cola de etiquetas". Pensada para
    /// juntar varios productos nuevos a lo largo del día (a veces son solo 3-5 prendas de una
    /// marca) y mandarlos todos juntos a una sola impresión en vez de desperdiciar una hoja por
    /// cada producto. No es un registro contable como AjusteStock/Compra/Venta — es scratch data
    /// transitoria, por eso no hereda de AuditEntity: se borra directo (sin baja lógica) al
    /// eliminarse o al imprimir toda la cola.
    /// </summary>
    public class EtiquetaPendiente
    {
        public int Id { get; set; }
        public int IdArticulo { get; set; }

        /// <summary>Cuántas copias de la etiqueta de este artículo hay que imprimir. Si se agrega
        /// el mismo artículo dos veces, se suma acá en vez de crear una fila nueva.</summary>
        public int Cantidad { get; set; }

        public DateTime FechaAgregado { get; set; }
        public string? UserAgregado { get; set; }

        public Articulo Articulo { get; set; } = null!;
    }
}
