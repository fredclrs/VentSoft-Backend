using Domain.Dtos;
using FluentValidation;

namespace Application.Validators
{
    public class ClienteDtoValidator : AbstractValidator<ClienteDto>
    {
        public ClienteDtoValidator()
        {
            RuleFor(c => c.Nombre).NotEmpty().WithMessage("El nombre es obligatorio.");
            RuleFor(c => c.DocumentoIdentidad).NotEmpty().WithMessage("El documento de identidad es obligatorio.");
            RuleFor(c => c.Correo)
                .EmailAddress().WithMessage("El formato del correo no es válido.")
                .When(c => !string.IsNullOrWhiteSpace(c.Correo));
        }
    }
}
