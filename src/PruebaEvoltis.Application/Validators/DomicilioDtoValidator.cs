using Domain.Dtos;
using FluentValidation;

namespace Application.Validators
{
    public class DomicilioDtoValidator : AbstractValidator<DomicilioDto>
    {
        public DomicilioDtoValidator()
        {
            RuleFor(d => d.Calle).NotEmpty().WithMessage("La calle es obligatoria.");
            RuleFor(d => d.Ciudad).NotEmpty().WithMessage("La ciudad es obligatoria.");
            RuleFor(d => d.Provincia).NotEmpty().WithMessage("La provincia es obligatoria.");
        }
    }
}