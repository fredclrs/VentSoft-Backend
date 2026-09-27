using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Common;
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

        /// <summary>
        /// Clasifica una línea contra el catálogo existente, dejando el resultado marcado en la
        /// misma línea (EsNuevo / EsVarianteNueva / IdArticuloExistente+CodigoExistente /
        /// CodigoGrupo+IdFamiliaGrupo):
        ///
        /// - Sin código compartido: alcanza con que coincida la Descripción (cada Código ya es
        ///   único, así que "mismo producto" = "mismo Artículo").
        /// - Con código compartido (mismo criterio que ya usa la carga manual — ver
        ///   AddArticuloCommandHandler/"+ Variante" en Artículos): coincide la Descripción PERO
        ///   además hay que fijarse la talla/color puntual, porque todas las variantes de una
        ///   misma prenda comparten la misma Descripción:
        ///     - Si además coincide la talla/color con una variante ya cargada → esa MISMA
        ///       variante (EsNuevo=false).
        ///     - Si la prenda ya existe pero esta talla/color todavía no → variante nueva del
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

            var tamanoBuscado = string.Join(" · ", new[] { linea.Talla, linea.Color }.Where(v => !string.IsNullOrWhiteSpace(v)));
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
