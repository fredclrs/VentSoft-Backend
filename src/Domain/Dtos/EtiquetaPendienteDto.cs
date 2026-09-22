namespace Domain.Dtos
{
    public class EtiquetaPendienteDto
    {
        public int Id { get; set; }
        public int IdArticulo { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaAgregado { get; set; }
    }
}
