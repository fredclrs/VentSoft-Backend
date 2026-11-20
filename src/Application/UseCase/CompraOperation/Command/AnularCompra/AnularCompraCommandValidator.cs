using FluentValidation;

namespace Application.UseCase.CompraOperation.Command.AnularCompra
{
    public class AnularCompraCommandValidator : AbstractValidator<AnularCompraCommand>
    {
        public AnularCompraCommandValidator()
        {
            RuleFor(x => x.IdCompra).GreaterThan(0).WithMessage("Falta la compra a anular.");
            // Mismo largo que la columna Compra.MotivoAnulacion.
            RuleFor(x => x.Motivo)
                .NotEmpty().WithMessage("Escribí el motivo de la anulación.")
                .MaximumLength(200).WithMessage("El motivo no puede tener más de 200 caracteres.");
        }
    }
}
