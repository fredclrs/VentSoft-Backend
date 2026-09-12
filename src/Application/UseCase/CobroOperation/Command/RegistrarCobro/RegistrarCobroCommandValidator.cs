using FluentValidation;

namespace Application.UseCase.CobroOperation.Command.RegistrarCobro
{
    public class RegistrarCobroCommandValidator : AbstractValidator<RegistrarCobroCommand>
    {
        public RegistrarCobroCommandValidator()
        {
            RuleFor(x => x.CobroDto).NotNull().WithMessage("El cobro es obligatorio.");

            When(x => x.CobroDto != null, () =>
            {
                RuleFor(x => x.CobroDto.IdCliente).GreaterThan(0).WithMessage("El cliente es obligatorio.");
                RuleFor(x => x.CobroDto.IdUsuario).GreaterThan(0).WithMessage("El usuario es obligatorio.");
                RuleFor(x => x.CobroDto.Monto).GreaterThan(0).WithMessage("El monto del cobro debe ser mayor a 0.");
            });
        }
    }
}
