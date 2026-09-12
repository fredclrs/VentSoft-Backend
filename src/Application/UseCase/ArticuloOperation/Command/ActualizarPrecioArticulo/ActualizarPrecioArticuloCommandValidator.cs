using FluentValidation;

namespace Application.UseCase.ArticuloOperation.Command.ActualizarPrecioArticulo
{
    public class ActualizarPrecioArticuloCommandValidator : AbstractValidator<ActualizarPrecioArticuloCommand>
    {
        public ActualizarPrecioArticuloCommandValidator()
        {
            RuleFor(x => x.IdArticulo).GreaterThan(0).WithMessage("El artículo es obligatorio.");
            RuleFor(x => x.Precio).GreaterThanOrEqualTo(0).WithMessage("El precio no puede ser negativo.");
        }
    }
}
