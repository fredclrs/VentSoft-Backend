using Domain.Dtos;
using MediatR;

namespace Application.UseCase.ConfiguracionEmpresaOperation.Command.UpdateConfiguracionEmpresa
{
    public class UpdateConfiguracionEmpresaCommand : IRequest<BaseResponse<ConfiguracionEmpresaDto>>
    {
        public string Nombre { get; set; } = null!;
        public string Moneda { get; set; } = null!;
        public bool PermiteVentaACredito { get; set; } = true;
        public bool PermiteCompraACredito { get; set; } = true;
        public bool RedondearPreciosEnteros { get; set; } = false;
        public bool PermiteCodigoCompartidoEntreArticulos { get; set; } = false;

        /// <summary>Solo si viene con un valor (no vacío): reemplaza la API key de IA guardada.
        /// Null = no se toca la que ya estaba configurada (el frontend nunca la muestra de vuelta,
        /// así que no la reenvía a menos que la persona pegue una nueva).</summary>
        public string? ClaveApiIA { get; set; }

        /// <summary>Si viene en true, borra la clave configurada (gana por sobre ClaveApiIA si
        /// por algún motivo vinieran los dos a la vez) — para el botón "Quitar clave".</summary>
        public bool EliminarClaveApiIA { get; set; }

        public int? IdClientePorDefecto { get; set; }
        public int? IdProveedorPorDefecto { get; set; }
    }
}
