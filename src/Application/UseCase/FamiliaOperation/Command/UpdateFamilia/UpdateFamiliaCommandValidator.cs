using FluentValidation;

namespace Application.UseCase.FamiliaOperation.Command.UpdateFamilia
{
    public class UpdateFamiliaCommandValidator : AbstractValidator<UpdateFamiliaCommand>
    {
        public UpdateFamiliaCommandValidator()
        {
            RuleFor(x => x.FamiliaDto).NotNull().WithMessage("La familia es obligatoria.");
            RuleFor(x => x.FamiliaDto.NombreFamilia)
                .NotEmpty().WithMessage("El campo NombreFamilia es obligatorio.")
                .When(x => x.FamiliaDto != null);
        }
    }
}
