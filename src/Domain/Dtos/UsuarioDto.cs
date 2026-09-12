namespace Domain.Dtos
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string DocumentoIdentidad { get; set; } = null!;
        public string? Nit { get; set; }
        public string? Direccion { get; set; }
        public string? Zona { get; set; }
        public int? Telefono { get; set; }
        public string? Correo { get; set; }
        public string? Nota { get; set; }
        public string NombreUsuario { get; set; } = null!;

        /// <summary>Si es true, tiene todos los permisos sin necesidad de tildar nada.</summary>
        public bool EsAdministrador { get; set; }

        /// <summary>Claves de Domain.Common.Permisos separadas por coma (solo relevante si EsAdministrador es false).</summary>
        public string Permisos { get; set; } = string.Empty;

        /// <summary>Solo se usa al crear/actualizar el usuario; nunca se devuelve en las respuestas.</summary>
        public string? Contrasena { get; set; }

        public string Estado { get; set; } = null!;
    }
}
