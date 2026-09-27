namespace Application.Interfaces
{
    /// <summary>Cliente genérico para mandarle un mensaje (texto, opcionalmente con una imagen) a
    /// Claude y recibir su respuesta como texto plano — cada función de IA (leer factura,
    /// interpretar una lista, leer un documento) arma su propio prompt pidiéndole al modelo que
    /// conteste en JSON, y parsea esa respuesta por su cuenta; este cliente no sabe nada de esa
    /// lógica, solo hace el llamado HTTP.</summary>
    public interface IAnthropicClient
    {
        /// <summary>
        /// Manda <paramref name="prompt"/> (y la imagen, si se pasa una) usando <paramref name="apiKey"/>
        /// (ya descifrada — ver ICifradoService) y devuelve el texto de la respuesta del modelo tal cual.
        /// Tira AnthropicApiException con un mensaje ya en español, listo para mostrar, si la clave es
        /// inválida, no hay crédito, o falla la conexión.
        /// </summary>
        Task<string> EnviarAsync(string apiKey, string prompt, ImagenAdjunta? imagen, CancellationToken cancellationToken = default);
    }

    /// <summary>Bytes crudos de una imagen (foto de factura, documento, etc.) + su tipo MIME
    /// (ej. "image/jpeg"), tal como la manda el navegador.</summary>
    public record ImagenAdjunta(byte[] Bytes, string MediaType);
}
