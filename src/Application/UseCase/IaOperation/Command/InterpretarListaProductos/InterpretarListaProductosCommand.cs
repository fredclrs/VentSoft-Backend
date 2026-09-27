using Domain.Dtos;
using MediatR;

namespace Application.UseCase.IaOperation.Command.InterpretarListaProductos
{
    public class InterpretarListaProductosCommand : IRequest<BaseResponse<List<LineaTextoProductoDto>>>
    {
        public string Texto { get; set; } = null!;
    }
}
