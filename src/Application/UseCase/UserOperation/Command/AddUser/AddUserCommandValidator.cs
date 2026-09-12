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

            RuleFor(x => x.UsuarioDto.Contrasena)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .When(x => x.UsuarioDto != null);
        }
    }
}
