
namespace Domain.Entities
{
    public class Usuario
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public ICollection<Domicilio> Domicilios { get; set; }
    }
}
