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

                // Si se está cobrando algo ahora, tiene que quedar clasificado cómo — si no, el
                // reporte "Ventas del día" no puede distinguir efectivo de tarjeta/QR para esta venta.
                RuleFor(x => x.VentaDto.IdFormaDePago)
                    .NotNull()
                    .When(x => x.VentaDto.Pagado > 0)
                    .WithMessage("Elegí una forma de pago para lo que se está cobrando ahora.");
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
