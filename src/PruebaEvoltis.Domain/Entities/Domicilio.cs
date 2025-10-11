
namespace Domain.Entities
{
    public class Domicilio
    {
        public int ID { get; set; }
        public int UsuarioID { get; set; }
        public string Calle { get; set; }
        public string Numero { get; set; }
        public string Provincia { get; set; }
        public string Ciudad { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public Usuario Usuario { get; set; }
    }
}
