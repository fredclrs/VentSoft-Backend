namespace Application.Interfaces
{
    /// <summary>Cualquier falla al llamar a la API de Anthropic (clave inválida, sin crédito,
    /// timeout, respuesta rara) — el Message ya viene en español, listo para devolver tal cual en
    /// un BaseResponse.FailureResponse, sin tener que interpretar el error en cada handler.</summary>
    public class AnthropicApiException : Exception
    {
        public AnthropicApiException(string message) : base(message) { }
    }
}
