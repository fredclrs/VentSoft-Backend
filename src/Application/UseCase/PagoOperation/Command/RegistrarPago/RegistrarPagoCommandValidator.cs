using FluentValidation;

namespace Application.UseCase.PagoOperation.Command.RegistrarPago
{
    public class RegistrarPagoCommandValidator : AbstractValidator<RegistrarPagoCommand>
    {
        public RegistrarPagoCommandValidator()
        {
            RuleFor(x => x.PagoDto).NotNull().WithMessage("El pago es obligatorio.");

            When(x => x.PagoDto != null, () =>
            {
                RuleFor(x => x.PagoDto.IdProveedor).GreaterThan(0).WithMessage("El proveedor es obligatorio.");
                RuleFor(x => x.PagoDto.IdUsuario).GreaterThan(0).WithMessage("El usuario es obligatorio.");
                RuleFor(x => x.PagoDto.Monto).GreaterThan(0).WithMessage("El monto del pago debe ser mayor a 0.");
            });
        }
    }
}
