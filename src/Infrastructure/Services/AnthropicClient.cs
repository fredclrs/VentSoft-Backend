using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using Application.Interfaces;

namespace Infrastructure.Services;

/// <summary>
/// Llama a la API de mensajes de Anthropic (https://api.anthropic.com/v1/messages) — primer uso
/// de HTTP saliente en este backend, no hay ningún patrón previo que imitar. Se registra como
/// "typed client" (ver AddHttpClient&lt;IAnthropicClient, AnthropicClient&gt; en DependencyInjection)
/// para que .NET le maneje el pooling de conexiones solo.
/// </summary>
public class AnthropicClient : IAnthropicClient
{
    // Buena precisión leyendo fotos reales (facturas con mala letra/luz, documentos de
    // identidad) — más importante acá que ahorrar unos centavos con un modelo más chico.
    private const string Modelo = "claude-sonnet-5";
    private const string ApiVersion = "2023-06-01";

    private readonly HttpClient _httpClient;

    public AnthropicClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress ??= new Uri("https://api.anthropic.com/");
        _httpClient.Timeout = TimeSpan.FromSeconds(60);
    }

    public async Task<string> EnviarAsync(string apiKey, string prompt, ImagenAdjunta? imagen, CancellationToken cancellationToken = default)
    {
        var contenido = new JsonArray();
        if (imagen != null)
        {
            contenido.Add(new JsonObject
            {
                ["type"] = "image",
                ["source"] = new JsonObject
                {
                    ["type"] = "base64",
                    ["media_type"] = imagen.MediaType,
                    ["data"] = Convert.ToBase64String(imagen.Bytes),
                },
            });
        }
        contenido.Add(new JsonObject { ["type"] = "text", ["text"] = prompt });

        var cuerpo = new JsonObject
        {
            ["model"] = Modelo,
            ["max_tokens"] = 4096,
            ["messages"] = new JsonArray
            {
                new JsonObject { ["role"] = "user", ["content"] = contenido },
            },
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, "v1/messages")
        {
            Content = JsonContent.Create(cuerpo),
        };
        request.Headers.Add("x-api-key", apiKey);
        request.Headers.Add("anthropic-version", ApiVersion);

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.SendAsync(request, cancellationToken);
        }
        catch (TaskCanceledException)
        {
            throw new AnthropicApiException("La IA tardó demasiado en responder. Probá de nuevo.");
        }
        catch (HttpRequestException)
        {
            throw new AnthropicApiException("No se pudo conectar con el servicio de IA. Revisá tu conexión a internet e intentá de nuevo.");
        }

        var textoRespuesta = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new AnthropicApiException(MensajeDeError(response.StatusCode, textoRespuesta));
        }

        JsonNode? json;
        try
        {
            json = JsonNode.Parse(textoRespuesta);
        }
        catch (JsonException)
        {
            throw new AnthropicApiException("La IA devolvió una respuesta que no se pudo leer. Probá de nuevo.");
        }

        var texto = json?["content"]?[0]?["text"]?.GetValue<string>();
        if (string.IsNullOrWhiteSpace(texto))
            throw new AnthropicApiException("La IA no devolvió ningún resultado. Probá de nuevo.");

        return texto;
    }

    private static string MensajeDeError(HttpStatusCode statusCode, string cuerpoRespuesta)
    {
        string? mensajeAnthropic = null;
        try
        {
            mensajeAnthropic = JsonNode.Parse(cuerpoRespuesta)?["error"]?["message"]?.GetValue<string>();
        }
        catch (JsonException)
        {
            // El cuerpo del error no vino en el formato esperado — seguimos con el mensaje genérico.
        }

        return statusCode switch
        {
            HttpStatusCode.Unauthorized =>
                "La clave de IA configurada no es válida. Revisala en Configuración del negocio.",
            HttpStatusCode.TooManyRequests or HttpStatusCode.PaymentRequired =>
                "No se pudo usar la IA: parece que no queda crédito en tu cuenta. Revisalo en console.anthropic.com.",
            _ => mensajeAnthropic != null
                ? $"No se pudo usar la IA: {mensajeAnthropic}"
                : "Ocurrió un error al usar la IA. Probá de nuevo en un momento.",
        };
    }
}
