using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Common
{
    /// <summary>Lo que repiten los 3 handlers de IA (leer factura, interpretar lista, leer
    /// documento de cliente): conseguir la clave configurada y descifrarla, y comparar una
    /// descripción leída/interpretada contra el catálogo existente.</summary>
    public static class IaHelpers
    {
        /// <summary>Null en ApiKey (con Error explicando por qué) si el negocio no configuró
        /// ninguna clave todavía.</summary>
        public static async Task<(string? ApiKey, string? Error)> ObtenerApiKeyAsync(
            IRepository<ConfiguracionEmpresa> configuracionRepository,
            ICifradoService cifradoService)
        {
            var configuracion = (await configuracionRepository.GetAllAsync()).FirstOrDefault();
            if (configuracion?.ClaveApiIACifrada == null)
                return (null, "Configurá tu clave de IA en Configuración del negocio antes de usar esto.");

            return (cifradoService.Descifrar(configuracion.ClaveApiIACifrada), null);
        }

        /// <summary>Comparación simple por texto (sin librerías de fuzzy-matching): cada
        /// descripción "contiene" a la otra, sin importar mayúsculas — alcanza para el caso común
        /// (la IA suele copiar el nombre casi textual de lo que lee/interpreta).</summary>
        public static Articulo? BuscarCoincidencia(string descripcionLeida, List<Articulo> articulos)
        {
            var buscada = descripcionLeida.Trim();
            if (buscada.Length == 0) return null;

            return articulos.FirstOrDefault(a =>
                !string.IsNullOrWhiteSpace(a.Descripcion) &&
                (a.Descripcion.Contains(buscada, StringComparison.OrdinalIgnoreCase) ||
                 buscada.Contains(a.Descripcion, StringComparison.OrdinalIgnoreCase)));
        }
    }
}
