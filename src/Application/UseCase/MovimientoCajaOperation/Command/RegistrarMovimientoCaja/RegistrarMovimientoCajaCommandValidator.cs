using FluentValidation;

namespace Application.UseCase.MovimientoCajaOperation.Command.RegistrarMovimientoCaja
{
    public class RegistrarMovimientoCajaCommandValidator : AbstractValidator<RegistrarMovimientoCajaCommand>
    {
        public RegistrarMovimientoCajaCommandValidator()
        {
            RuleFor(x => x.MovimientoDto).NotNull().WithMessage("El movimiento es obligatorio.");

            When(x => x.MovimientoDto != null, () =>
            {
                RuleFor(x => x.MovimientoDto.IdUsuario).GreaterThan(0).WithMessage("El usuario es obligatorio.");
                RuleFor(x => x.MovimientoDto.Monto).GreaterThan(0).WithMessage("El monto debe ser mayor a 0.");
                RuleFor(x => x.MovimientoDto.Motivo).NotEmpty().WithMessage("El motivo es obligatorio.")
                    .MaximumLength(200).WithMessage("El motivo no puede superar los 200 caracteres.");
                RuleFor(x => x.MovimientoDto.Tipo)
                    .Must(t => t == "ENTRADA" || t == "SALIDA")
                    .WithMessage("El tipo debe ser ENTRADA o SALIDA.");
            });
        }
    }
}
