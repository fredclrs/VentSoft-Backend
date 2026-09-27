namespace Domain.Dtos
{
    /// <summary>Una foto mandada desde el celular/compu (factura, documento de identidad, etc.)
    /// para que la lea la IA — codificada en Base64, como la manda el navegador desde un
    /// FileReader.readAsDataURL (sin el prefijo "data:image/...;base64,", eso se saca en el
    /// frontend antes de mandarla).</summary>
    public class ImagenRequestDto
    {
        public string ImagenBase64 { get; set; } = null!;

        /// <summary>Ej. "image/jpeg", "image/png" — tal como viene del archivo elegido.</summary>
        public string MediaType { get; set; } = null!;
    }
}
