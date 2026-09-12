using FluentValidation;

namespace Application.UseCase.TemporadaOperation.Command.AddTemporada
{
    public class AddTemporadaCommandValidator : AbstractValidator<AddTemporadaCommand>
    {
        public AddTemporadaCommandValidator()
        {
            RuleFor(x => x.TemporadaDto).NotNull().WithMessage("La temporada es obligatoria.");
            RuleFor(x => x.TemporadaDto.Nombre)
                .NotEmpty().WithMessage("El campo Nombre es obligatorio.")
                .When(x => x.TemporadaDto != null);
            RuleFor(x => x.TemporadaDto.MesInicio)
                .InclusiveBetween(1, 12).WithMessage("El mes de inicio debe estar entre 1 y 12.")
                .When(x => x.TemporadaDto != null);
            RuleFor(x => x.TemporadaDto.MesFin)
                .InclusiveBetween(1, 12).WithMessage("El mes de fin debe estar entre 1 y 12.")
                .When(x => x.TemporadaDto != null);
        }
    }
}
