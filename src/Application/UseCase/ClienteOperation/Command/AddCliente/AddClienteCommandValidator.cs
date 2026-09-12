using Application.Validators;
using FluentValidation;

namespace Application.UseCase.ClienteOperation.Command.AddCliente
{
    public class AddClienteCommandValidator : AbstractValidator<AddClienteCommand>
    {
        public AddClienteCommandValidator()
        {
            RuleFor(x => x.ClienteDto)
                .NotNull().WithMessage("El cliente es obligatorio.")
                .SetValidator(new ClienteDtoValidator());
        }
    }
}
