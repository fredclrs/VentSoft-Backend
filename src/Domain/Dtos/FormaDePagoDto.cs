namespace Domain.Dtos
{
    public class FormaDePagoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public bool EsEfectivo { get; set; }
        public double? PorcentajeRecargo { get; set; }
        public string Estado { get; set; } = null!;
    }
}
