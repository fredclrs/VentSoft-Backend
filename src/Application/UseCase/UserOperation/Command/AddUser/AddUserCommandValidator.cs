using Application.Validators;
using FluentValidation;

namespace Application.UseCase.UserOperation.Command.AddUser
{
    public class AddUserCommandValidator : AbstractValidator<AddUserCommand>
    {
        public AddUserCommandValidator()
        {
            RuleFor(x => x.UsuarioDto)
                .NotNull().WithMessage("El usuario es obligatorio.")
                .SetValidator(new UsuarioDtoValidator());
        }
    }
}
