using Application.Validators;
using FluentValidation;

namespace Application.UseCase.ArticuloOperation.Command.AddArticulo
{
    public class AddArticuloCommandValidator : AbstractValidator<AddArticuloCommand>
    {
        public AddArticuloCommandValidator()
        {
            RuleFor(x => x.ArticuloDto)
                .NotNull().WithMessage("El artículo es obligatorio.")
                .SetValidator(new ArticuloDtoValidator());
        }
    }
}
