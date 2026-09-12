using FluentValidation;

namespace Application.UseCase.TipoBienOperation.Command.UpdateTipoBien
{
    public class UpdateTipoBienCommandValidator : AbstractValidator<UpdateTipoBienCommand>
    {
        public UpdateTipoBienCommandValidator()
        {
            RuleFor(x => x.TipoBienDto).NotNull().WithMessage("El tipo de bien es obligatorio.");

            When(x => x.TipoBienDto != null, () =>
            {
                RuleFor(x => x.TipoBienDto.Nombre).NotEmpty().WithMessage("El campo Nombre es obligatorio.");
                RuleFor(x => x.TipoBienDto.UnidadMedida).NotEmpty().WithMessage("El campo Unidad de medida es obligatorio.");
            });
        }
    }
}
