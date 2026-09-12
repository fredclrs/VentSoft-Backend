# 🧠 VentSoft

Aplicación .NET 8 en **Clean Architecture** para la gestión de compra-venta e inventario de negocios pequeños (clientes, proveedores, artículos, compras, ventas, cobros y pagos).

Incluye pruebas unitarias con **xUnit**, **Moq** y **AutoFixture**.

---

## 🚀 Tecnologías utilizadas

- .NET 8
- C#
- MediatR (CQRS)
- AutoMapper
- FluentValidation
- xUnit
- Moq
- AutoFixture
- SQL Server (EF Core, Database-First sobre `ScriptVentSoft... / SQLQueryVentSoft.sql`)

---

## ⚙️ Requisitos previos

Asegúrate de tener instalado:

- [.NET SDK 8.0](https://dotnet.microsoft.com/en-us/download)
- Visual Studio 2022
- Git
- SQL Server (LocalDB, Express o full)

---

## 🛠️ Cómo levantar la aplicación

1. Clonar el repositorio:
   ```bash
   git clone https://github.com/fredclrs/VentSoft-Backend.git
   ```

2. Crear la base de datos ejecutando el script `SQLQueryVentSoft.sql` contra tu instancia de SQL Server (crea la base `VentSoft` y todas sus tablas: Cliente, Usuario, Proveedor, FormaDePago, Familia, Caracteristica, Promocion, Articulo, Compra, DetalleCompra, Venta, DetalleVenta, Cobro, Pago).

3. Configurar la cadena de conexión en `src/WebVentSoft.API/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=VentSoft;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

4. Ejecutar la API (`dotnet run --project src/WebVentSoft.API`) y abrir Swagger.

---

## 🐳 Levantar todo con Docker (backend + frontend + base de datos)

Alternativa a los pasos de arriba: `docker-compose.yml` levanta los 3 servicios de una
sola vez, sin instalar nada más que Docker.

Requiere tener el repo [VentSoft-FrontEnd](https://github.com/fredclrs/VentSoft-FrontEnd)
clonado **justo al lado** de este (mismo directorio padre, no uno adentro del otro):

```
algún-directorio/
  ├── VentSoft-Backend/   ← este repo, acá corrés los comandos
  └── VentSoft-FrontEnd/
```

```bash
cp .env.example .env   # completá los 3 valores (contraseñas + clave JWT)
docker compose up -d --build
```

- Frontend: http://localhost:8080
- API/Swagger: http://localhost:5187/swagger
- SQL Server: `localhost,1433`

La primera vez, el servicio `db-init` corre `SQLQueryVentSoft.sql` solo y crea el
usuario de base dedicado para la API — no hay que tocar SSMS a mano. El admin
inicial (`admin` / `admin123`) lo crea la propia API si la tabla `Usuario` está vacía.

---

## Los tests cubren los siguientes handlers:

- AddUserCommandHandler
- DeleteUserCommandHandler
- UpdateUserCommandHandler
- SearchUserQueryHandler

Cada prueba valida el comportamiento en casos exitosos y de error.

---

## Base de datos (Database-First)

La base de datos ya existe en SQL Server y las entidades del proyecto están alineadas a ese esquema (no se usan migraciones de EF Core para crearla; el script `SQLQueryVentSoft.sql` es la fuente de verdad). Si el esquema cambia, actualizar primero la base con SQL y luego reflejar el cambio en `Domain/Entities` y `Infrastructure/Persistence/Configurations`.

---

AUTOR
Fredy Claro Rojas
