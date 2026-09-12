using Application.Validators;
using FluentValidation;

namespace Application.UseCase.ClienteOperation.Command.UpdateCliente
{
    public class UpdateClienteCommandValidator : AbstractValidator<UpdateClienteCommand>
    {
        public UpdateClienteCommandValidator()
        {
            RuleFor(x => x.ClienteDto)
                .NotNull().WithMessage("El cliente es obligatorio.")
                .SetValidator(new ClienteDtoValidator());
        }
    }
}
