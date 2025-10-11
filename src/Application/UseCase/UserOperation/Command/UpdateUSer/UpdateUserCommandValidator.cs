using Application.Validators;
using FluentValidation;

namespace Application.UseCase.UserOperation.Command.UpdateUSer
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator() {
            RuleFor(x => x.UsuarioDto)
                .NotNull().WithMessage("El usuario es obligatorio.")
                .SetValidator(new UsuarioDtoValidator());
        }
    }
}
