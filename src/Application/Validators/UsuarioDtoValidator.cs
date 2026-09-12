using System;
using System.Linq;
using Domain.Common;
using Domain.Dtos;
using FluentValidation;

namespace Application.Validators
{
    public class UsuarioDtoValidator : AbstractValidator<UsuarioDto>
    {
        public UsuarioDtoValidator()
        {
            RuleFor(u => u.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.");

            RuleFor(u => u.DocumentoIdentidad)
                .NotEmpty().WithMessage("El documento de identidad es obligatorio.");

            RuleFor(u => u.NombreUsuario)
                .NotEmpty().WithMessage("El nombre de usuario es obligatorio.");

            RuleFor(u => u.Permisos)
                .Must(permisos =>
                    string.IsNullOrWhiteSpace(permisos) ||
                    permisos.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                        .All(p => Permisos.Todos.Contains(p)))
                .WithMessage("Hay un permiso desconocido en la lista.");

            RuleFor(u => u.Correo)
                .EmailAddress().WithMessage("El formato del correo no es válido.")
                .When(u => !string.IsNullOrWhiteSpace(u.Correo));
        }
    }
}
