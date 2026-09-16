using Domain.Dtos;
using FluentValidation;

namespace Application.Validators
{
    public class ArticuloDtoValidator : AbstractValidator<ArticuloDto>
    {
        public ArticuloDtoValidator()
        {
            RuleFor(a => a.Codigo).NotEmpty().WithMessage("El código es obligatorio.");
            RuleFor(a => a.Tamano).NotEmpty().WithMessage("El campo Tamaño es obligatorio.");
            RuleFor(a => a.IdFamilia).GreaterThan(0).WithMessage("La familia (categoría) es obligatoria.");
            RuleFor(a => a.Precio).GreaterThanOrEqualTo(0).WithMessage("El precio no puede ser negativo.");
            RuleFor(a => a.Costo).GreaterThanOrEqualTo(0).WithMessage("El costo no puede ser negativo.");
            RuleFor(a => a.Fraccion).GreaterThanOrEqualTo(1).WithMessage("Las unidades por caja deben ser 1 o más.");
            RuleFor(a => a.PrecioUnidadSuelta)
                .GreaterThanOrEqualTo(0).When(a => a.PrecioUnidadSuelta.HasValue)
                .WithMessage("El precio por unidad suelta no puede ser negativo.");
            RuleFor(a => a.MargenGanancia)
                .GreaterThanOrEqualTo(0).When(a => a.MargenGanancia.HasValue)
                .WithMessage("El margen de ganancia no puede ser negativo.");
            RuleFor(a => a.MargenGanancia)
                .LessThan(100).When(a => a.MargenGanancia.HasValue)
                .WithMessage("El margen de ganancia debe ser menor a 100 (es sobre el precio de venta, no puede ser el 100% o más).");

            RuleForEach(a => a.Caracteristicas).ChildRules(c =>
            {
                c.RuleFor(x => x.IdCaracteristica).GreaterThan(0).WithMessage("La característica es obligatoria.");
                c.RuleFor(x => x.Valor).NotEmpty().WithMessage("El valor del atributo es obligatorio.");
            });
        }
    }
}
