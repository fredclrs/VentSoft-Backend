using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.ArticuloOperation.Command.UpdateArticulo
{
    public class UpdateArticuloCommandHandler : IRequestHandler<UpdateArticuloCommand, BaseResponse<ArticuloDto>>
    {
        private readonly IArticuloRepository _repository;
        private readonly IRepository<ConfiguracionEmpresa> _configuracionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateArticuloCommandHandler> _logger;

        public UpdateArticuloCommandHandler(
            IArticuloRepository repository,
            IRepository<ConfiguracionEmpresa> configuracionRepository,
            IMapper mapper,
            ILogger<UpdateArticuloCommandHandler> logger)
        {
            _repository = repository;
            _configuracionRepository = configuracionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<ArticuloDto>> Handle(UpdateArticuloCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var articulo = await _repository.GetByIdWithCaracteristicasAsync(request.Id);
                if (articulo == null)
                    return BaseResponse<ArticuloDto>.FailureResponse("Artículo no encontrado.");

                // Solo hace falta re-chequear si el Código o el Tamaño realmente cambian (si no
                // cambió nada de eso, da lo mismo el estado del flag — sigue siendo la misma
                // combinación que ya era válida). Ver AddArticuloCommandHandler para el mismo
                // chequeo en el alta.
                var codigoCambio = !string.Equals(articulo.Codigo, request.ArticuloDto.Codigo, StringComparison.Ordinal);
                var tamanoCambio = !string.Equals(articulo.Tamano?.Trim(), request.ArticuloDto.Tamano?.Trim(), StringComparison.OrdinalIgnoreCase);
                if (codigoCambio || tamanoCambio)
                {
                    var permiteCompartido = (await _configuracionRepository.GetAllAsync())
                        .FirstOrDefault()?.PermiteCodigoCompartidoEntreArticulos ?? false;
                    if (!permiteCompartido)
                    {
                        if (codigoCambio)
                        {
                            var existente = await _repository.GetByCodigoAsync(request.ArticuloDto.Codigo);
                            if (existente != null && existente.Id != articulo.Id)
                                return BaseResponse<ArticuloDto>.FailureResponse($"Ya existe un artículo con el código '{request.ArticuloDto.Codigo}'.");
                        }
                    }
                    else
                    {
                        // El código se puede repetir entre variantes — pero no la MISMA variante
                        // (mismo Tamaño) bajo ese código.
                        var existentes = await _repository.GetAllByCodigoAsync(request.ArticuloDto.Codigo);
                        var yaExisteVariante = existentes.Any(a =>
                            a.Id != articulo.Id &&
                            string.Equals(a.Tamano?.Trim(), request.ArticuloDto.Tamano?.Trim(), StringComparison.OrdinalIgnoreCase));
                        if (yaExisteVariante)
                            return BaseResponse<ArticuloDto>.FailureResponse(
                                $"Ya existe un artículo con el código '{request.ArticuloDto.Codigo}' y la misma variante ('{request.ArticuloDto.Tamano}').");
                    }
                }

                _mapper.Map(request.ArticuloDto, articulo);
                articulo.UserActualizado = "system"; // TODO: usuario autenticado real
                articulo.FechaActualizado = DateTime.Now;

                // Reemplaza el set completo de atributos: simple y predecible para un negocio chico.
                articulo.Caracteristicas.Clear();
                foreach (var c in request.ArticuloDto.Caracteristicas)
                {
                    articulo.Caracteristicas.Add(new ArticuloCaracteristica
                    {
                        IdCaracteristica = c.IdCaracteristica,
                        Valor = c.Valor
                    });
                }

                _repository.Update(articulo);
                await _repository.SaveChangesAsync();

                var actualizado = await _repository.GetByIdWithCaracteristicasAsync(articulo.Id);
                return BaseResponse<ArticuloDto>.SuccessResponse(_mapper.Map<ArticuloDto>(actualizado), "Artículo actualizado correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar artículo");
                return BaseResponse<ArticuloDto>.FailureResponse("Ocurrió un error al actualizar el artículo.");
            }
        }
    }
}
