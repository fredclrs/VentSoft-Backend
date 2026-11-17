using FluentValidation;

namespace Application.UseCase.CompraOperation.Command.ActualizarLoteDetalle
{
    public class ActualizarLoteDetalleCommandValidator : AbstractValidator<ActualizarLoteDetalleCommand>
    {
        public ActualizarLoteDetalleCommandValidator()
        {
            RuleFor(x => x.IdDetalleCompra).GreaterThan(0).WithMessage("Falta el renglón de la compra.");
            // Mismo largo que la columna DetalleCompra.Lote.
            RuleFor(x => x.Lote).MaximumLength(30).WithMessage("El lote no puede tener más de 30 caracteres.");
        }
    }
}
