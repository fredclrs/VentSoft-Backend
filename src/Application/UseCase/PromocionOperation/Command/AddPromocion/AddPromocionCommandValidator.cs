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

            // "N por $X": o se cargan la cantidad y el precio, o ninguno de los dos.
            RuleFor(x => x.PromocionDto.CantidadPromo)
                .GreaterThanOrEqualTo(2).WithMessage("La cantidad de la promoción tiene que ser 2 o más.")
                .When(x => x.PromocionDto != null && x.PromocionDto.CantidadPromo.HasValue);
            RuleFor(x => x.PromocionDto.PrecioPromo)
                .GreaterThan(0).WithMessage("El precio de la promoción tiene que ser mayor a 0.")
                .When(x => x.PromocionDto != null && x.PromocionDto.PrecioPromo.HasValue);
            RuleFor(x => x.PromocionDto)
                .Must(p => p.CantidadPromo.HasValue == p.PrecioPromo.HasValue)
                .WithMessage("Para una promoción \"N por precio\" cargá la cantidad y el precio.")
                .When(x => x.PromocionDto != null);
        }
    }
}
