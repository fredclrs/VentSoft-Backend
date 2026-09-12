using FluentValidation;

namespace Application.UseCase.PromocionOperation.Command.AddPromocion
{
    public class AddPromocionCommandValidator : AbstractValidator<AddPromocionCommand>
    {
        public AddPromocionCommandValidator()
        {
            RuleFor(x => x.PromocionDto).NotNull().WithMessage("La promoción es obligatoria.");
            RuleFor(x => x.PromocionDto.NombrePromocion)
                .NotEmpty().WithMessage("El campo NombrePromocion es obligatorio.")
                .When(x => x.PromocionDto != null);
        }
    }
}
