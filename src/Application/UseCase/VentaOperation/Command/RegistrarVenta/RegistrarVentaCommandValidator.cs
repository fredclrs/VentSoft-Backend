using FluentValidation;

namespace Application.UseCase.VentaOperation.Command.RegistrarVenta
{
    public class RegistrarVentaCommandValidator : AbstractValidator<RegistrarVentaCommand>
    {
        public RegistrarVentaCommandValidator()
        {
            RuleFor(x => x.VentaDto).NotNull().WithMessage("La venta es obligatoria.");

            When(x => x.VentaDto != null, () =>
            {
                RuleFor(x => x.VentaDto.IdCliente).GreaterThan(0).WithMessage("El cliente es obligatorio.");
                RuleFor(x => x.VentaDto.IdUsuario).GreaterThan(0).WithMessage("El usuario es obligatorio.");
                RuleFor(x => x.VentaDto.Pagado).GreaterThanOrEqualTo(0).WithMessage("El monto pagado no puede ser negativo.");
                RuleFor(x => x.VentaDto.MontoSaldoAFavorAplicado).GreaterThanOrEqualTo(0).WithMessage("El saldo a favor aplicado no puede ser negativo.");
                RuleFor(x => x.VentaDto.Detalles).NotEmpty().WithMessage("La venta debe tener al menos un artículo.");

                RuleForEach(x => x.VentaDto.Detalles).ChildRules(d =>
                {
                    d.RuleFor(x => x.IdArticulo).GreaterThan(0).WithMessage("El artículo es obligatorio.");
                    d.RuleFor(x => x.Cantidad).GreaterThan(0).WithMessage("La cantidad debe ser mayor a 0.");
                    d.RuleFor(x => x.PrecioUnitario).GreaterThanOrEqualTo(0).WithMessage("El precio unitario no puede ser negativo.");
                });
            });
        }
    }
}
