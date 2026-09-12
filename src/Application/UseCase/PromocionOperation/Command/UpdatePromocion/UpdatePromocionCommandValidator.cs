using FluentValidation;

namespace Application.UseCase.PromocionOperation.Command.UpdatePromocion
{
    public class UpdatePromocionCommandValidator : AbstractValidator<UpdatePromocionCommand>
    {
        public UpdatePromocionCommandValidator()
        {
            RuleFor(x => x.PromocionDto).NotNull().WithMessage("La promoción es obligatoria.");
            RuleFor(x => x.PromocionDto.NombrePromocion)
                .NotEmpty().WithMessage("El campo NombrePromocion es obligatorio.")
                .When(x => x.PromocionDto != null);
        }
    }
}
