using System.Security.Cryptography;
using System.Text;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

/// <summary>
/// Cifrado simétrico (AES) para datos sensibles que hace falta poder leer de vuelta (API keys de
/// IA, etc.) — a diferencia de PasswordHasher, que es de un solo sentido. La clave sale de
/// configuración ("Encryption:Key", variable de entorno ENCRYPTION_KEY en el .env de cada
/// instalación) — NUNCA del código — así que si esa clave no está configurada, cifrar/descifrar
/// tira un error claro en vez de fallar en silencio con una clave adivinada.
///
/// Formato guardado: Base64(IV de 16 bytes + texto cifrado) — un IV nuevo y al azar por cada
/// cifrado, como corresponde con AES-CBC, para que cifrar el mismo texto dos veces no dé siempre
/// el mismo resultado.
/// </summary>
public class CifradoService : ICifradoService
{
    private readonly byte[] _clave;

    public CifradoService(IConfiguration configuration)
    {
        var claveConfigurada = configuration["Encryption:Key"];
        if (string.IsNullOrWhiteSpace(claveConfigurada))
            throw new InvalidOperationException(
                "Falta configurar 'Encryption:Key' (variable de entorno ENCRYPTION_KEY) — hace falta para poder guardar la API key de IA de forma segura.");

        // SHA-256 del texto configurado: da siempre exactamente 32 bytes (AES-256), sin importar
        // qué tan larga sea la clave que se haya puesto en el .env.
        using var sha = SHA256.Create();
        _clave = sha.ComputeHash(Encoding.UTF8.GetBytes(claveConfigurada));
    }

    public string Cifrar(string textoPlano)
    {
        using var aes = Aes.Create();
        aes.Key = _clave;
        aes.GenerateIV();

        using var encryptor = aes.CreateEncryptor();
        var textoBytes = Encoding.UTF8.GetBytes(textoPlano);
        var cifrado = encryptor.TransformFinalBlock(textoBytes, 0, textoBytes.Length);

        var resultado = new byte[aes.IV.Length + cifrado.Length];
        Buffer.BlockCopy(aes.IV, 0, resultado, 0, aes.IV.Length);
        Buffer.BlockCopy(cifrado, 0, resultado, aes.IV.Length, cifrado.Length);
        return Convert.ToBase64String(resultado);
    }

    public string Descifrar(string textoCifrado)
    {
        var datos = Convert.FromBase64String(textoCifrado);

        using var aes = Aes.Create();
        aes.Key = _clave;
        var iv = new byte[16];
        Buffer.BlockCopy(datos, 0, iv, 0, iv.Length);
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor();
        var plano = decryptor.TransformFinalBlock(datos, iv.Length, datos.Length - iv.Length);
        return Encoding.UTF8.GetString(plano);
    }
}
