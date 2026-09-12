using Domain.Dtos;
using FluentValidation;

namespace Application.Validators
{
    public class ProveedorDtoValidator : AbstractValidator<ProveedorDto>
    {
        public ProveedorDtoValidator()
        {
            RuleFor(p => p.Nombre).NotEmpty().WithMessage("El nombre es obligatorio.");
            RuleFor(p => p.Correo)
                .EmailAddress().WithMessage("El formato del correo no es válido.")
                .When(p => !string.IsNullOrWhiteSpace(p.Correo));
        }
    }
}
