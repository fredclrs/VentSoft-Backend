using Application.Validators;
using FluentValidation;

namespace Application.UseCase.ProveedorOperation.Command.UpdateProveedor
{
    public class UpdateProveedorCommandValidator : AbstractValidator<UpdateProveedorCommand>
    {
        public UpdateProveedorCommandValidator()
        {
            RuleFor(x => x.ProveedorDto)
                .NotNull().WithMessage("El proveedor es obligatorio.")
                .SetValidator(new ProveedorDtoValidator());
        }
    }
}
