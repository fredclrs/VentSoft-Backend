namespace Domain.Entities
{
    /// <summary>
    /// Configuración general del negocio que usa esta instancia de VentSoft: el nombre (barra
    /// superior y comprobantes impresos), el símbolo de moneda (Bs., $, U$S, etc., usado en
    /// todos los montos que se muestran o imprimen), si el negocio vende a crédito y el
    /// cliente/proveedor por defecto para ventas/compras rápidas. Es una tabla de una sola fila
    /// (no hay multi-empresa): si no existe, se crea sola con valores por defecto la primera vez
    /// que se consulta.
    /// </summary>
    public class ConfiguracionEmpresa
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "VentSoft";
        public string Moneda { get; set; } = "Bs.";

        /// <summary>Si es false, el negocio solo vende al contado: el check "Contado" de la
        /// pantalla de Ventas queda siempre marcado y bloqueado, nadie puede desmarcarlo.</summary>
        public bool PermiteVentaACredito { get; set; } = true;

        /// <summary>Si es false, el negocio solo compra al contado: el check "Contado" de la
        /// pantalla de Compras queda siempre marcado y bloqueado, nadie puede desmarcarlo.</summary>
        public bool PermiteCompraACredito { get; set; } = true;

        /// <summary>Cliente que se preselecciona al entrar a Ventas (para no obligar a cargar
        /// datos de cada cliente ocasional en temporada alta). Opcional: null si no se configuró.</summary>
        public int? IdClientePorDefecto { get; set; }

        /// <summary>Proveedor que se preselecciona al entrar a Compras. Opcional.</summary>
        public int? IdProveedorPorDefecto { get; set; }

        /// <summary>Si es true, el precio de venta calculado automáticamente por margen de
        /// ganancia (en Artículos y al sugerir precio nuevo después de una Compra) se redondea
        /// al número entero de arriba (Math.Ceiling) en vez de a 2 decimales — para negocios que
        /// no manejan centavos. Redondea siempre PARA ARRIBA a propósito: así nunca se pierde
        /// margen de ganancia por el redondeo.</summary>
        public bool RedondearPreciosEnteros { get; set; } = false;

        /// <summary>Si es true, varios Artículos pueden compartir el mismo Código de barras —
        /// para cuando el proveedor imprime un solo código por línea de producto en vez de uno
        /// por variante puntual (el caso típico es indumentaria: una prenda en varias
        /// tallas/colores con un solo código impreso para todas, pero puede pasar en otros
        /// rubros — ej. ferretería con distintos tintes de una misma pintura). Cada variante
        /// sigue siendo un Artículo separado (con su propio stock), solo cambia que el Código ya
        /// no tiene que ser único entre ellos. Con esto en false (por defecto, y lo que conviene
        /// para la mayoría de los rubros — farmacia en particular, donde cada presentación tiene
        /// su propio código de fábrica), el Código sigue siendo único como siempre — cero cambio
        /// de comportamiento.</summary>
        public bool PermiteCodigoCompartidoEntreArticulos { get; set; } = false;
    }
}
