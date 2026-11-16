using Application.Interfaces.Repositories;
using Domain.Dtos;

namespace Application.Common
{
    /// <summary>Chequeo compartido por alta y edición de artículo: un código escaneado tiene que
    /// llevar a UN solo artículo sin ambigüedad, así que el código de la caja (CodigoPaquete) no
    /// puede coincidir con el código ni con el código de caja de ningún OTRO artículo, y el código
    /// de la unidad tampoco puede coincidir con el código de caja de otro.</summary>
    public static class CodigoPaqueteHelpers
    {
        /// <summary>Null si está todo bien; si no, el mensaje de error para el usuario.
        /// idActual: el artículo que se está editando (null en un alta).</summary>
        public static async Task<string?> ValidarAsync(IArticuloRepository repository, ArticuloDto dto, int? idActual)
        {
            var codigo = dto.Codigo?.Trim();
            var codigoPaquete = string.IsNullOrWhiteSpace(dto.CodigoPaquete) ? null : dto.CodigoPaquete.Trim();

            var otros = (await repository.GetAllAsync()).Where(a => a.Id != idActual).ToList();

            if (codigoPaquete != null)
            {
                var choca = otros.FirstOrDefault(a =>
                    string.Equals(a.Codigo, codigoPaquete, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(a.CodigoPaquete, codigoPaquete, StringComparison.OrdinalIgnoreCase));
                if (choca != null)
                    return $"El código de la caja '{codigoPaquete}' ya lo usa el artículo '{choca.Descripcion ?? choca.Codigo}'.";
            }

            var cajaDeOtro = otros.FirstOrDefault(a =>
                string.Equals(a.CodigoPaquete, codigo, StringComparison.OrdinalIgnoreCase));
            if (cajaDeOtro != null)
                return $"El código '{codigo}' ya está cargado como código de la caja del artículo '{cajaDeOtro.Descripcion ?? cajaDeOtro.Codigo}'.";

            return null;
        }
    }
}
