using System.Text.Json;

namespace Application.Common
{
    /// <summary>
    /// El modelo de IA a veces contesta el JSON pedido envuelto en explicación o en un bloque
    /// ```json ... ``` a pesar de que el prompt le pide que no lo haga — esto rescata el array
    /// JSON de en medio de cualquier texto extra antes de intentar deserializarlo, en vez de
    /// romper directo si la respuesta no es JSON puro.
    /// </summary>
    public static class JsonExtractor
    {
        public static List<T> ExtraerArray<T>(string textoRespuesta)
        {
            var inicio = textoRespuesta.IndexOf('[');
            var fin = textoRespuesta.LastIndexOf(']');
            if (inicio == -1 || fin == -1 || fin < inicio)
                throw new JsonException("No se encontró una lista JSON en la respuesta.");

            var soloJson = textoRespuesta.Substring(inicio, fin - inicio + 1);
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<List<T>>(soloJson, opciones)
                ?? throw new JsonException("La lista JSON vino vacía o no se pudo interpretar.");
        }

        /// <summary>Igual que ExtraerArray pero para un solo objeto JSON (ej. los datos de un
        /// documento de identidad), no una lista.</summary>
        public static T ExtraerObjeto<T>(string textoRespuesta)
        {
            var inicio = textoRespuesta.IndexOf('{');
            var fin = textoRespuesta.LastIndexOf('}');
            if (inicio == -1 || fin == -1 || fin < inicio)
                throw new JsonException("No se encontró un objeto JSON en la respuesta.");

            var soloJson = textoRespuesta.Substring(inicio, fin - inicio + 1);
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<T>(soloJson, opciones)
                ?? throw new JsonException("El objeto JSON no se pudo interpretar.");
        }
    }
}
