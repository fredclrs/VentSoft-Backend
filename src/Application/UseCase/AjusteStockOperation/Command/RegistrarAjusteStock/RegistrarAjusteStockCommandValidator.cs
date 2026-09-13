using FluentValidation;

namespace Application.UseCase.AjusteStockOperation.Command.RegistrarAjusteStock
{
    public class RegistrarAjusteStockCommandValidator : AbstractValidator<RegistrarAjusteStockCommand>
    {
        public RegistrarAjusteStockCommandValidator()
        {
            RuleFor(x => x.AjusteDto).NotNull().WithMessage("El ajuste es obligatorio.");

            When(x => x.AjusteDto != null, () =>
            {
                RuleFor(x => x.AjusteDto.IdArticulo).GreaterThan(0).WithMessage("El artículo es obligatorio.");
                RuleFor(x => x.AjusteDto.IdUsuario).GreaterThan(0).WithMessage("El usuario es obligatorio.");
                RuleFor(x => x.AjusteDto.Cantidad).GreaterThan(0).WithMessage("La cantidad debe ser mayor a 0.");
                RuleFor(x => x.AjusteDto.Motivo).NotEmpty().WithMessage("El motivo es obligatorio.")
                    .MaximumLength(200).WithMessage("El motivo no puede superar los 200 caracteres.");
                RuleFor(x => x.AjusteDto.Tipo)
                    .Must(t => t == "ENTRADA" || t == "SALIDA")
                    .WithMessage("El tipo debe ser ENTRADA o SALIDA.");
            });
        }
    }
}
