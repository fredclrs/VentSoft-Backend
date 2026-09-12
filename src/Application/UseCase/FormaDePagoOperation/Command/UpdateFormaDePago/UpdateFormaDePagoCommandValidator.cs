using FluentValidation;

namespace Application.UseCase.FormaDePagoOperation.Command.UpdateFormaDePago
{
    public class UpdateFormaDePagoCommandValidator : AbstractValidator<UpdateFormaDePagoCommand>
    {
        public UpdateFormaDePagoCommandValidator()
        {
            RuleFor(x => x.FormaDePagoDto).NotNull().WithMessage("La forma de pago es obligatoria.");
            RuleFor(x => x.FormaDePagoDto.Nombre)
                .NotEmpty().WithMessage("El campo Nombre es obligatorio.")
                .When(x => x.FormaDePagoDto != null);
        }
    }
}
