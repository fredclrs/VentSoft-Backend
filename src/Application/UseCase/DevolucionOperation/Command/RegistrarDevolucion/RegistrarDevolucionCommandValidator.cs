using FluentValidation;

namespace Application.UseCase.DevolucionOperation.Command.RegistrarDevolucion
{
    public class RegistrarDevolucionCommandValidator : AbstractValidator<RegistrarDevolucionCommand>
    {
        public RegistrarDevolucionCommandValidator()
        {
            RuleFor(x => x.DevolucionDto).NotNull().WithMessage("La devolución es obligatoria.");

            When(x => x.DevolucionDto != null, () =>
            {
                RuleFor(x => x.DevolucionDto.IdVenta).GreaterThan(0).WithMessage("La venta original es obligatoria.");
                RuleFor(x => x.DevolucionDto.IdUsuario).GreaterThan(0).WithMessage("El usuario es obligatorio.");
                RuleFor(x => x.DevolucionDto.Detalles).NotEmpty().WithMessage("Debe indicar al menos un artículo a devolver.");
                RuleFor(x => x.DevolucionDto.MontoCobradoAhora).GreaterThanOrEqualTo(0).WithMessage("El monto cobrado no puede ser negativo.");

                RuleForEach(x => x.DevolucionDto.Detalles).ChildRules(d =>
                {
                    d.RuleFor(x => x.IdDetalleVenta).GreaterThan(0).WithMessage("El renglón de la venta original es obligatorio.");
                    d.RuleFor(x => x.Cantidad).GreaterThan(0).WithMessage("La cantidad a devolver debe ser mayor a 0.");
                });

                RuleForEach(x => x.DevolucionDto.ArticulosCambio).ChildRules(a =>
                {
                    a.RuleFor(x => x.IdArticulo).GreaterThan(0).WithMessage("El artículo es obligatorio.");
                    a.RuleFor(x => x.Cantidad).GreaterThan(0).WithMessage("La cantidad debe ser mayor a 0.");
                    a.RuleFor(x => x.PrecioUnitario).GreaterThanOrEqualTo(0).WithMessage("El precio unitario no puede ser negativo.");
                });
            });
        }
    }
}
