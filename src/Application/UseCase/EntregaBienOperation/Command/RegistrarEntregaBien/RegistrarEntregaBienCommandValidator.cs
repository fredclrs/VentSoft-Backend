using FluentValidation;

namespace Application.UseCase.EntregaBienOperation.Command.RegistrarEntregaBien
{
    public class RegistrarEntregaBienCommandValidator : AbstractValidator<RegistrarEntregaBienCommand>
    {
        public RegistrarEntregaBienCommandValidator()
        {
            RuleFor(x => x.EntregaDto).NotNull().WithMessage("La entrega es obligatoria.");

            When(x => x.EntregaDto != null, () =>
            {
                RuleFor(x => x.EntregaDto.IdCliente).GreaterThan(0).WithMessage("El cliente es obligatorio.");
                RuleFor(x => x.EntregaDto.IdUsuario).GreaterThan(0).WithMessage("El usuario es obligatorio.");
                RuleFor(x => x.EntregaDto.IdTipoBien).GreaterThan(0).WithMessage("El tipo de bien es obligatorio.");
                RuleFor(x => x.EntregaDto.Cantidad).GreaterThan(0).WithMessage("La cantidad debe ser mayor a 0.");
                RuleFor(x => x.EntregaDto.PrecioUnitario)
                    .GreaterThanOrEqualTo(0).When(x => x.EntregaDto.PrecioUnitario.HasValue)
                    .WithMessage("El precio unitario no puede ser negativo.");
            });
        }
    }
}
