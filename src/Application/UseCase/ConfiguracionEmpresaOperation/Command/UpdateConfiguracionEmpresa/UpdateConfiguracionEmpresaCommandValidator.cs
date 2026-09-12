using FluentValidation;

namespace Application.UseCase.ConfiguracionEmpresaOperation.Command.UpdateConfiguracionEmpresa
{
    public class UpdateConfiguracionEmpresaCommandValidator : AbstractValidator<UpdateConfiguracionEmpresaCommand>
    {
        public UpdateConfiguracionEmpresaCommandValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre del negocio es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre del negocio no puede superar los 100 caracteres.");

            RuleFor(x => x.Moneda)
                .NotEmpty().WithMessage("El símbolo de moneda es obligatorio.")
                .MaximumLength(10).WithMessage("El símbolo de moneda no puede superar los 10 caracteres.");

            RuleFor(x => x.IdClientePorDefecto)
                .GreaterThan(0).When(x => x.IdClientePorDefecto.HasValue)
                .WithMessage("El cliente por defecto no es válido.");

            RuleFor(x => x.IdProveedorPorDefecto)
                .GreaterThan(0).When(x => x.IdProveedorPorDefecto.HasValue)
                .WithMessage("El proveedor por defecto no es válido.");
        }
    }
}
