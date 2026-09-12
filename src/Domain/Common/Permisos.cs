namespace Domain.Common
{
    /// <summary>
    /// Catálogo de permisos individuales que el Administrador puede otorgar a cada usuario
    /// (no hay roles fijos: cada usuario tiene exactamente los permisos que se le tildaron).
    /// Un usuario con EsAdministrador=true los tiene todos automáticamente, sin necesidad de
    /// tildar nada — ver AuthService.GenerateToken.
    /// </summary>
    public static class Permisos
    {
        public const string Ventas = "ventas";
        public const string Compras = "compras";
        public const string Cobros = "cobros";
        public const string Pagos = "pagos";
        public const string CuentasPorCobrar = "cuentas_cobrar";
        public const string CuentasPorPagar = "cuentas_pagar";
        public const string Inventario = "inventario";
        public const string VentasDelDia = "ventas_dia";
        public const string Usuarios = "usuarios";
        public const string Configuracion = "configuracion";

        /// <summary>Registrar boletas de entrega de bienes (pago en especie) — operativo, sin
        /// impacto en la deuda todavía (eso pasa recién al liquidar).</summary>
        public const string Entregas = "entregas";

        /// <summary>Liquidar entregas (fijar precio y aplicarlas contra la deuda del cliente) —
        /// más sensible que registrar la boleta, por eso es un permiso aparte.</summary>
        public const string Liquidaciones = "liquidaciones";

        /// <summary>Ver las ventas de TODOS los vendedores (ranking de desempeño, historial por
        /// vendedor, trazabilidad por artículo) — información sensible sobre el personal, por
        /// eso es un permiso aparte del de Ventas (uno puede vender sin poder ver esto).</summary>
        public const string VentasPorVendedor = "ventas_vendedor";

        public static readonly string[] Todos =
        {
            Ventas, Compras, Cobros, Pagos, CuentasPorCobrar, CuentasPorPagar,
            Inventario, VentasDelDia, Usuarios, Configuracion, Entregas, Liquidaciones,
            VentasPorVendedor,
        };
    }
}
