namespace Domain.Dtos
{
    /// <summary>Lo que la IA leyó de la foto de un documento de identidad — la persona lo revisa
    /// y corrige antes de guardar el Cliente, no se crea nada solo.</summary>
    public class DatosClienteExtraidosDto
    {
        public string? Nombre { get; set; }
        public string? DocumentoIdentidad { get; set; }
    }
}
