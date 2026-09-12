using FluentValidation;

namespace Application.UseCase.CompraOperation.Command.RegistrarCompra
{
    public class RegistrarCompraCommandValidator : AbstractValidator<RegistrarCompraCommand>
    {
        public RegistrarCompraCommandValidator()
        {
            RuleFor(x => x.CompraDto).NotNull().WithMessage("La compra es obligatoria.");

            When(x => x.CompraDto != null, () =>
            {
                RuleFor(x => x.CompraDto.IdProveedor).GreaterThan(0).WithMessage("El proveedor es obligatorio.");
                RuleFor(x => x.CompraDto.IdUsuario).GreaterThan(0).WithMessage("El usuario es obligatorio.");
                RuleFor(x => x.CompraDto.Pagado).GreaterThanOrEqualTo(0).WithMessage("El monto pagado no puede ser negativo.");
                RuleFor(x => x.CompraDto.Detalles).NotEmpty().WithMessage("La compra debe tener al menos un artículo.");

                RuleForEach(x => x.CompraDto.Detalles).ChildRules(d =>
                {
                    d.RuleFor(x => x.IdArticulo).GreaterThan(0).WithMessage("El artículo es obligatorio.");
                    d.RuleFor(x => x.Cantidad).GreaterThan(0).WithMessage("La cantidad debe ser mayor a 0.");
                    d.RuleFor(x => x.CostoUnitario).GreaterThanOrEqualTo(0).WithMessage("El costo unitario no puede ser negativo.");
                });
            });
        }
    }
}
