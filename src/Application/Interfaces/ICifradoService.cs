namespace Application.Interfaces
{
    /// <summary>Cifrado simétrico reversible para datos sensibles que el sistema necesita poder
    /// leer de vuelta en texto plano (a diferencia de IPasswordHasher, que es de un solo sentido:
    /// una contraseña de usuario nunca hace falta recuperarla, pero una API key sí, para poder
    /// usarla al llamar a la IA).</summary>
    public interface ICifradoService
    {
        string Cifrar(string textoPlano);
        string Descifrar(string textoCifrado);
    }
}
