using FluentValidation;

namespace Application.UseCase.LiquidacionOperation.Command.RegistrarLiquidacion
{
    public class RegistrarLiquidacionCommandValidator : AbstractValidator<RegistrarLiquidacionCommand>
    {
        public RegistrarLiquidacionCommandValidator()
        {
            RuleFor(x => x.LiquidacionDto).NotNull().WithMessage("La liquidación es obligatoria.");

            When(x => x.LiquidacionDto != null, () =>
            {
                RuleFor(x => x.LiquidacionDto.IdCliente).GreaterThan(0).WithMessage("El cliente es obligatorio.");
                RuleFor(x => x.LiquidacionDto.IdUsuario).GreaterThan(0).WithMessage("El usuario es obligatorio.");
                RuleFor(x => x.LiquidacionDto.Entregas).NotEmpty().WithMessage("Debe incluir al menos una entrega.");

                RuleForEach(x => x.LiquidacionDto.Entregas).ChildRules(e =>
                {
                    e.RuleFor(x => x.IdEntrega).GreaterThan(0).WithMessage("La entrega es obligatoria.");
                    e.RuleFor(x => x.PrecioUnitario).GreaterThan(0).WithMessage("El precio de cada entrega es obligatorio para liquidar.");
                });
            });
        }
    }
}
