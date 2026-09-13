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
        public int? IdClientePorDefecto { get; set; }
        public int? IdProveedorPorDefecto { get; set; }
    }
}
