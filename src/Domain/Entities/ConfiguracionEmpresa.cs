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
    }
}
