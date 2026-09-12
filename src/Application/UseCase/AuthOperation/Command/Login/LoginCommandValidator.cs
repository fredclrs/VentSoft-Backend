using FluentValidation;

namespace Application.UseCase.AuthOperation.Command.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.LoginRequestDto)
                .NotNull().WithMessage("Las credenciales son obligatorias.");

            RuleFor(x => x.LoginRequestDto!.NombreUsuario)
                .NotEmpty().WithMessage("El nombre de usuario es obligatorio.")
                .When(x => x.LoginRequestDto != null);

            RuleFor(x => x.LoginRequestDto!.Contrasena)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .When(x => x.LoginRequestDto != null);
        }
    }
}
