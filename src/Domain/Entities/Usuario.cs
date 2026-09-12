using Domain.Common;

namespace Domain.Entities
{
    public class Usuario : AuditEntity
    {
        public string Nombre { get; set; } = null!;
        public string DocumentoIdentidad { get; set; } = null!;
        public string? Nit { get; set; }
        public string? Direccion { get; set; }
        public string? Zona { get; set; }
        public int? Telefono { get; set; }
        public string? Correo { get; set; }
        public string? Nota { get; set; }

        public string NombreUsuario { get; set; } = null!;

        /// <summary>Si es true, tiene todos los permisos sin necesidad de tildar nada (ver Domain.Common.Permisos).</summary>
        public bool EsAdministrador { get; set; }

        /// <summary>
        /// Permisos individuales otorgados (claves de Domain.Common.Permisos separadas por coma),
        /// solo relevante cuando EsAdministrador es false. No hay roles fijos: el Administrador
        /// decide puntualmente qué puede hacer cada usuario.
        /// </summary>
        public string Permisos { get; set; } = string.Empty;

        /// <summary>Hash de la contraseña (ver IPasswordHasher). No se persiste en texto plano.</summary>
        public string Contrasena { get; set; } = null!;

        /// <summary>
        /// Columna heredada de la BD actual, redundante con Contrasena.
        /// Se recomienda eliminarla vía ALTER TABLE (ver notas del proyecto).
        /// </summary>
        public string ConfirmarContrasena { get; set; } = null!;

        public ICollection<Compra> Compras { get; set; } = new List<Compra>();
        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
        public ICollection<Cobro> Cobros { get; set; } = new List<Cobro>();
        public ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    }
}
