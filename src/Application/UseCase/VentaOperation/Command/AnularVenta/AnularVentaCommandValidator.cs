using FluentValidation;

namespace Application.UseCase.VentaOperation.Command.AnularVenta
{
    public class AnularVentaCommandValidator : AbstractValidator<AnularVentaCommand>
    {
        public AnularVentaCommandValidator()
        {
            RuleFor(x => x.IdVenta).GreaterThan(0).WithMessage("Falta la venta a anular.");
            // Mismo largo que la columna Venta.MotivoAnulacion.
            RuleFor(x => x.Motivo)
                .NotEmpty().WithMessage("Escribí el motivo de la anulación.")
                .MaximumLength(200).WithMessage("El motivo no puede tener más de 200 caracteres.");
        }
    }
}
