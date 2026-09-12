using FluentValidation;

namespace Application.UseCase.FormaDePagoOperation.Command.AddFormaDePago
{
    public class AddFormaDePagoCommandValidator : AbstractValidator<AddFormaDePagoCommand>
    {
        public AddFormaDePagoCommandValidator()
        {
            RuleFor(x => x.FormaDePagoDto).NotNull().WithMessage("La forma de pago es obligatoria.");
            RuleFor(x => x.FormaDePagoDto.Nombre)
                .NotEmpty().WithMessage("El campo Nombre es obligatorio.")
                .When(x => x.FormaDePagoDto != null);
        }
    }
}
