namespace Domain.Dtos
{
    /// <summary>Datos para registrar una boleta de entrega. PrecioUnitario es opcional: en
    /// muchos rubros (agro) el precio del bien recién se define al liquidar.</summary>
    public class RegistrarEntregaBienDto
    {
        public DateTime Fecha { get; set; }
        public string? NumeroBoleta { get; set; }
        public double Cantidad { get; set; }
        public double? PrecioUnitario { get; set; }
        public string? Nota { get; set; }

        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }
        public int IdTipoBien { get; set; }
    }
}
