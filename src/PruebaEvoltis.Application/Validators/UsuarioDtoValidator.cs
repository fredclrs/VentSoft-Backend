using Domain.Dtos;
using FluentValidation;

namespace Application.Validators
{
    public class UsuarioDtoValidator : AbstractValidator<UsuarioDto>
    {
        public UsuarioDtoValidator()
        {
            RuleFor(u => u.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("El email es obligatorio.")
                .EmailAddress().WithMessage("El formato del email no es válido.");

            RuleForEach(u => u.Domicilios)
                .SetValidator(new DomicilioDtoValidator());
        }
    }
}