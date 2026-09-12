# syntax=docker/dockerfile:1

# ---- Build ----
# El contexto de build es esta carpeta (VentSoft/), porque los .csproj se referencian
# entre sí con rutas relativas (../Application, etc.) — hace falta tener todo src/ junto.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia solo los .csproj primero: mientras no cambien las dependencias, Docker cachea
# el "dotnet restore" y no lo vuelve a correr en cada build por un cambio de código.
COPY src/Domain/Domain.csproj src/Domain/
COPY src/Application/Application.csproj src/Application/
COPY src/Infrastructure/Infrastructure.csproj src/Infrastructure/
COPY src/WebVentSoft.API/WebVentSoft.API.csproj src/WebVentSoft.API/
RUN dotnet restore src/WebVentSoft.API/WebVentSoft.API.csproj

# Recién acá el resto del código.
COPY src/ src/
RUN dotnet publish src/WebVentSoft.API/WebVentSoft.API.csproj -c Release -o /app --no-restore

# ---- Runtime ----
# Imagen de runtime nomás (sin el SDK completo): mucho más liviana para lo que
# efectivamente se despliega.
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Kestrel escucha en 8080 dentro del contenedor (no hace falta root para ese puerto,
# a diferencia del 80/443). El mapeo al puerto que uses en tu máquina se define en
# docker-compose.yml.
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app .
ENTRYPOINT ["dotnet", "WebVentSoft.API.dll"]
