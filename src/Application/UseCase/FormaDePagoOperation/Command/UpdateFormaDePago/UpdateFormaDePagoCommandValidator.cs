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
            RuleFor(x => x.FormaDePagoDto.PorcentajeRecargo)
                .GreaterThanOrEqualTo(0).WithMessage("El recargo no puede ser negativo.")
                .When(x => x.FormaDePagoDto != null && x.FormaDePagoDto.PorcentajeRecargo.HasValue);
        }
    }
}
