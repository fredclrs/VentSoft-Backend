using FluentValidation;

namespace Application.UseCase.EtiquetaPendienteOperation.Command.AgregarEtiquetaPendiente
{
    public class AgregarEtiquetaPendienteCommandValidator : AbstractValidator<AgregarEtiquetaPendienteCommand>
    {
        public AgregarEtiquetaPendienteCommandValidator()
        {
            RuleFor(x => x.EtiquetaDto).NotNull().WithMessage("Los datos son obligatorios.");

            When(x => x.EtiquetaDto != null, () =>
            {
                RuleFor(x => x.EtiquetaDto.IdArticulo).GreaterThan(0).WithMessage("El artículo es obligatorio.");
                RuleFor(x => x.EtiquetaDto.Cantidad).GreaterThan(0).WithMessage("La cantidad debe ser mayor a 0.");
            });
        }
    }
}
