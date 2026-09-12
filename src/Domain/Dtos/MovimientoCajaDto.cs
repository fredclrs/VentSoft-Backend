namespace Domain.Dtos
{
    public class MovimientoCajaDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; } = null!;
        public double Monto { get; set; }
        public string Motivo { get; set; } = null!;
        public int IdUsuario { get; set; }
        public string Estado { get; set; } = null!;
    }
}
