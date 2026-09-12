
namespace Application.Interfaces
{
    public interface IVentaService
    {
        Task<int> RegistrarVentaAsync(int idCliente, int idUsuario, List<(int idArticulo, double cantidad)> items);
    }
}
