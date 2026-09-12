using FluentValidation;

namespace Application.UseCase.CaracteristicaOperation.Command.UpdateCaracteristica
{
    public class UpdateCaracteristicaCommandValidator : AbstractValidator<UpdateCaracteristicaCommand>
    {
        public UpdateCaracteristicaCommandValidator()
        {
            RuleFor(x => x.CaracteristicaDto).NotNull().WithMessage("La característica es obligatoria.");
            RuleFor(x => x.CaracteristicaDto.NombreCaracteristica)
                .NotEmpty().WithMessage("El campo NombreCaracteristica es obligatorio.")
                .When(x => x.CaracteristicaDto != null);
        }
    }
}
