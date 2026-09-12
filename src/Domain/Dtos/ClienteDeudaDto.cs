namespace Domain.Dtos
{
    public class ClienteDeudaDto
    {
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; } = null!;
        public double DeudaActual { get; set; }

        /// <summary>Crédito del cliente por devoluciones/cambios no devueltos en efectivo — se puede
        /// aplicar como pago en una futura venta (no se resta acá de DeudaActual, se muestra aparte).</summary>
        public double SaldoAFavor { get; set; }
    }
}
