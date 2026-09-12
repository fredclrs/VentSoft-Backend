namespace Domain.Dtos
{
    /// <summary>Un renglón a devolver de la venta original.</summary>
    public class RegistrarDetalleDevolucionDto
    {
        /// <summary>Id del DetalleVenta original del que se está devolviendo.</summary>
        public int IdDetalleVenta { get; set; }
        public int Cantidad { get; set; }

        /// <summary>Si el artículo vuelve a stock vendible (true) o no, por estar dañado, sin etiqueta, etc.</summary>
        public bool Vendible { get; set; } = true;
    }
}
