using FluentValidation;

namespace Application.UseCase.CaracteristicaOperation.Command.AddCaracteristica
{
    public class AddCaracteristicaCommandValidator : AbstractValidator<AddCaracteristicaCommand>
    {
        public AddCaracteristicaCommandValidator()
        {
            RuleFor(x => x.CaracteristicaDto).NotNull().WithMessage("La característica es obligatoria.");
            RuleFor(x => x.CaracteristicaDto.NombreCaracteristica)
                .NotEmpty().WithMessage("El campo NombreCaracteristica es obligatorio.")
                .When(x => x.CaracteristicaDto != null);
        }
    }
}
