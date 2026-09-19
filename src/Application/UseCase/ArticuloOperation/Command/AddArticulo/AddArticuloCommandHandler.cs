using Application.Interfaces;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCase.ArticuloOperation.Command.AddArticulo
{
    public class AddArticuloCommandHandler : IRequestHandler<AddArticuloCommand, BaseResponse<ArticuloDto>>
    {
        private readonly IArticuloRepository _repository;
        private readonly IRepository<ConfiguracionEmpresa> _configuracionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AddArticuloCommandHandler> _logger;

        public AddArticuloCommandHandler(
            IArticuloRepository repository,
            IRepository<ConfiguracionEmpresa> configuracionRepository,
            IMapper mapper,
            ILogger<AddArticuloCommandHandler> logger)
        {
            _repository = repository;
            _configuracionRepository = configuracionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<ArticuloDto>> Handle(AddArticuloCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Código único, salvo que el negocio haya activado explícitamente compartirlo
                // entre variantes (talla/color) de un mismo modelo — ver
                // ConfiguracionEmpresa.PermiteCodigoCompartidoEntreArticulos.
                var permiteCompartido = (await _configuracionRepository.GetAllAsync())
                    .FirstOrDefault()?.PermiteCodigoCompartidoEntreArticulos ?? false;
                if (!permiteCompartido)
                {
                    var existente = await _repository.GetByCodigoAsync(request.ArticuloDto.Codigo);
                    if (existente != null)
                        return BaseResponse<ArticuloDto>.FailureResponse($"Ya existe un artículo con el código '{request.ArticuloDto.Codigo}'.");
                }
                else
                {
                    // El código se puede repetir entre variantes — pero no la MISMA variante
                    // (mismo Tamaño) bajo ese código, porque ahí sí sería un duplicado real
                    // por error, no una variante nueva.
                    var existentes = await _repository.GetAllByCodigoAsync(request.ArticuloDto.Codigo);
                    var yaExisteVariante = existentes.Any(a =>
                        string.Equals(a.Tamano?.Trim(), request.ArticuloDto.Tamano?.Trim(), StringComparison.OrdinalIgnoreCase));
                    if (yaExisteVariante)
                        return BaseResponse<ArticuloDto>.FailureResponse(
                            $"Ya existe un artículo con el código '{request.ArticuloDto.Codigo}' y la misma variante ('{request.ArticuloDto.Tamano}').");
                }

                var articulo = _mapper.Map<Articulo>(request.ArticuloDto);
                articulo.Estado = "AC";
                articulo.FechaRegistro = DateTime.Now;

                foreach (var c in request.ArticuloDto.Caracteristicas)
                {
                    articulo.Caracteristicas.Add(new ArticuloCaracteristica
                    {
                        IdCaracteristica = c.IdCaracteristica,
                        Valor = c.Valor
                    });
                }

                await _repository.AddAsync(articulo);
                await _repository.SaveChangesAsync();

                var creado = await _repository.GetByIdWithCaracteristicasAsync(articulo.Id);
                return BaseResponse<ArticuloDto>.SuccessResponse(_mapper.Map<ArticuloDto>(creado), "Artículo agregado correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar artículo");
                return BaseResponse<ArticuloDto>.FailureResponse("Ocurrió un error al agregar el artículo.");
            }
        }
    }
}
