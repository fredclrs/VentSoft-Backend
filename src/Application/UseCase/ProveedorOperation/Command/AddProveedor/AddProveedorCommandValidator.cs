using Application.Validators;
using FluentValidation;

namespace Application.UseCase.ProveedorOperation.Command.AddProveedor
{
    public class AddProveedorCommandValidator : AbstractValidator<AddProveedorCommand>
    {
        public AddProveedorCommandValidator()
        {
            RuleFor(x => x.ProveedorDto)
                .NotNull().WithMessage("El proveedor es obligatorio.")
                .SetValidator(new ProveedorDtoValidator());
        }
    }
}
