using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Common;
using Domain.Dtos;
using Domain.Entities;

namespace Application.Common
{
    /// <summary>Lo que repiten los handlers de IA que comparan contra el catálogo (leer factura,
    /// interpretar lista): conseguir la clave configurada y descifrarla, y clasificar cada línea
    /// leída/interpretada.</summary>
    public static class IaHelpers
    {
        /// <summary>Null en ApiKey (con Error explicando por qué) si el negocio no configuró
        /// ninguna clave todavía. PermiteCodigoCompartido se necesita para Clasificar.</summary>
        public static async Task<(string? ApiKey, bool PermiteCodigoCompartido, string? Error)> ObtenerApiKeyAsync(
            IRepository<ConfiguracionEmpresa> configuracionRepository,
            ICifradoService cifradoService)
        {
            var configuracion = (await configuracionRepository.GetAllAsync()).FirstOrDefault();
            if (configuracion?.ClaveApiIACifrada == null)
                return (null, false, "Configurá tu clave de IA en Configuración del negocio antes de usar esto.");

            return (cifradoService.Descifrar(configuracion.ClaveApiIACifrada), configuracion.PermiteCodigoCompartidoEntreArticulos, null);
        }

        /// <summary>Para cada Caracteristica que la IA devolvió por nombre (nombreCaracteristica),
        /// busca el IdCaracteristica real del catálogo del negocio (coincidencia exacta primero,
        /// después "contiene" en cualquier sentido, ambas sin distinguir mayúsculas) — así el
        /// artículo se puede guardar con el mismo mecanismo que el alta manual. Si no encuentra
        /// ninguna coincidencia deja IdCaracteristica en 0 (la persona la corrige a mano en la
        /// pantalla de revisión, el Valor leído no se pierde).</summary>
        public static void ResolverCaracteristicas(List<ArticuloCaracteristicaDto> caracteristicas, List<Caracteristica> catalogo)
        {
            foreach (var c in caracteristicas)
            {
                if (string.IsNullOrWhiteSpace(c.NombreCaracteristica)) continue;

                var nombre = c.NombreCaracteristica.Trim();
                var match = catalogo.FirstOrDefault(cat => cat.NombreCaracteristica.Equals(nombre, StringComparison.OrdinalIgnoreCase))
                    ?? catalogo.FirstOrDefault(cat =>
                        cat.NombreCaracteristica.Contains(nombre, StringComparison.OrdinalIgnoreCase) ||
                        nombre.Contains(cat.NombreCaracteristica, StringComparison.OrdinalIgnoreCase));

                if (match != null)
                {
                    c.IdCaracteristica = match.Id;
                    c.NombreCaracteristica = match.NombreCaracteristica;
                }
            }
        }

        /// <summary>
        /// Clasifica una línea contra el catálogo existente, dejando el resultado marcado en la
        /// misma línea (EsNuevo / EsVarianteNueva / IdArticuloExistente+CodigoExistente /
        /// CodigoGrupo+IdFamiliaGrupo):
        ///
        /// - Sin código compartido: alcanza con que coincida la Descripción (cada Código ya es
        ///   único, así que "mismo producto" = "mismo Artículo").
        /// - Con código compartido (mismo criterio que ya usa la carga manual — ver
        ///   AddArticuloCommandHandler/"+ Variante" en Artículos): coincide la Descripción PERO
        ///   además hay que fijarse las Caracteristicas puntuales (talla/color/lo que use el
        ///   negocio), porque todas las variantes de un mismo producto comparten la misma
        ///   Descripción:
        ///     - Si además coincide el Tamaño (las Caracteristicas armadas igual que en el alta
        ///       manual) con una variante ya cargada → esa MISMA variante (EsNuevo=false).
        ///     - Si el producto ya existe pero esta variante todavía no → variante nueva del
        ///       MISMO código (EsVarianteNueva=true, con CodigoGrupo/IdFamiliaGrupo para no
        ///       tener que inventar un código nuevo).
        ///     - Si no coincide ninguna Descripción → producto totalmente nuevo (EsNuevo=true).
        /// </summary>
        public static void Clasificar(ILineaClasificable linea, List<Articulo> articulos, bool permiteCodigoCompartido)
        {
            var candidatos = BuscarPorDescripcion(linea.Descripcion, articulos);
            if (candidatos.Count == 0)
            {
                linea.EsNuevo = true;
                return;
            }

            if (!permiteCodigoCompartido)
            {
                var unico = candidatos[0];
                linea.EsNuevo = false;
                linea.IdArticuloExistente = unico.Id;
                linea.CodigoExistente = unico.Codigo;
                return;
            }

            // Mismo criterio que tamanoDeFila en el alta manual: une los Valor de las
            // Caracteristicas con " · ", en el orden en que vinieron.
            var tamanoBuscado = string.Join(" · ", linea.Caracteristicas
                .Select(c => c.Valor?.Trim())
                .Where(v => !string.IsNullOrWhiteSpace(v)));
            var varianteExacta = !string.IsNullOrWhiteSpace(tamanoBuscado)
                ? candidatos.FirstOrDefault(a => TamanosCoinciden(a.Tamano, tamanoBuscado))
                : null;

            if (varianteExacta != null)
            {
                linea.EsNuevo = false;
                linea.IdArticuloExistente = varianteExacta.Id;
                linea.CodigoExistente = varianteExacta.Codigo;
                return;
            }

            // La prenda ya existe (mismo código+descripción+familia) pero esta talla/color no —
            // es una variante nueva, no un producto nuevo: reusa el código del grupo.
            var grupo = candidatos[0];
            linea.EsVarianteNueva = true;
            linea.CodigoGrupo = grupo.Codigo;
            linea.IdFamiliaGrupo = grupo.IdFamilia;
        }

        /// <summary>Fragmento común para armar el prompt de LeerFactura/InterpretarListaProductos:
        /// le explica a la IA qué atributos de variante usa ESTE negocio en particular (talla y
        /// color en indumentaria, pero puede ser material/lote/lo que sea en otro rubro) para que
        /// devuelva "caracteristicas" con esos nombres exactos en vez de inventar talla/color
        /// siempre.</summary>
        public static string DescribirCaracteristicasParaPrompt(List<Caracteristica> catalogo)
        {
            if (catalogo.Count == 0)
            {
                return "Este negocio no tiene atributos de variante configurados (no separa por " +
                       "talla/color/lote/etc.) — dejá \"caracteristicas\" como un array vacío [] " +
                       "en todos los elementos.";
            }

            var nombres = string.Join(", ", catalogo.Select(c => $"\"{c.NombreCaracteristica}\""));
            return "Este negocio distingue variantes de un mismo producto por estos atributos: " +
                   nombres + ". La clave \"caracteristicas\" es un array de objetos " +
                   "{\"nombreCaracteristica\": <uno de esos nombres, EXACTAMENTE como está escrito " +
                   "arriba, ni un acento ni una letra distinta>, \"valor\": <el valor puntual, ej. " +
                   "\"L\" o \"Azul\">} — incluí solo los atributos que el texto/imagen realmente " +
                   "menciona para esa línea (podés dejarlo vacío si no menciona ninguno), y nunca " +
                   "un nombre de atributo que no esté en la lista de arriba.";
        }

        /// <summary>Comparación simple por texto (sin librerías de fuzzy-matching): cada
        /// descripción "contiene" a la otra, sin importar mayúsculas — alcanza para el caso común
        /// (la IA suele copiar el nombre casi textual de lo que lee/interpreta).</summary>
        private static List<Articulo> BuscarPorDescripcion(string descripcionLeida, List<Articulo> articulos)
        {
            var buscada = descripcionLeida.Trim();
            if (buscada.Length == 0) return new List<Articulo>();

            return articulos.Where(a =>
                !string.IsNullOrWhiteSpace(a.Descripcion) &&
                (a.Descripcion.Contains(buscada, StringComparison.OrdinalIgnoreCase) ||
                 buscada.Contains(a.Descripcion, StringComparison.OrdinalIgnoreCase))).ToList();
        }

        private static bool TamanosCoinciden(string tamanoExistente, string tamanoBuscado)
        {
            var existente = tamanoExistente.Trim();
            var buscado = tamanoBuscado.Trim();
            return existente.Equals(buscado, StringComparison.OrdinalIgnoreCase) ||
                   existente.Contains(buscado, StringComparison.OrdinalIgnoreCase) ||
                   buscado.Contains(existente, StringComparison.OrdinalIgnoreCase);
        }
    }
}
