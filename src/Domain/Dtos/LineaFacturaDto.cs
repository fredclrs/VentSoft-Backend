namespace Domain.Dtos
{
    /// <summary>Un renglón que la IA leyó de la foto de una factura de compra, ya comparado
    /// contra el catálogo — la persona lo revisa/corrige antes de confirmar, nunca se crea ni se
    /// agrega nada solo.</summary>
    public class LineaFacturaDto
    {
        public string Descripcion { get; set; } = null!;
        public int Cantidad { get; set; }
        public double CostoUnitario { get; set; }

        /// <summary>true si no se encontró ningún artículo parecido en el catálogo — hace falta
        /// completar Código/Familia/Tamaño para poder darlo de alta.</summary>
        public bool EsNuevo { get; set; }

        /// <summary>Solo si EsNuevo es false: el artículo del catálogo que coincidió.</summary>
        public int? IdArticuloExistente { get; set; }
        public string? CodigoExistente { get; set; }
    }
}
