using Application.Validators;
using FluentValidation;

namespace Application.UseCase.ArticuloOperation.Command.UpdateArticulo
{
    public class UpdateArticuloCommandValidator : AbstractValidator<UpdateArticuloCommand>
    {
        public UpdateArticuloCommandValidator()
        {
            RuleFor(x => x.ArticuloDto)
                .NotNull().WithMessage("El artículo es obligatorio.")
                .SetValidator(new ArticuloDtoValidator());
        }
    }
}
