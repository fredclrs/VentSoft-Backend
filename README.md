# 🧠 Prueba Evoltis

Este proyecto es una aplicación .NET 8 Clean Arquiture que implementa operaciones CRUD para la gestión de usuarios (alta, baja, modificación y búsqueda).  
Incluye pruebas unitarias con **xUnit**, **Moq** y **AutoFixture**.

---

## 🚀 Tecnologías utilizadas

- .NET 8
- C#
- MediatR
- AutoMapper
- FluentValidation
- xUnit
- Moq
- AutoFixture
- MySQL

---

## ⚙️ Requisitos previos

Asegúrate de tener instalado:

- [.NET SDK 8.0](https://dotnet.microsoft.com/en-us/download)
- Visual Studio 2022
- Git
- MySQL Server

---

## 🛠️ Cómo levantar la aplicación

1. Clonar el repositorio:
   ```bash
   git clone https://github.com/fredclrs/PruebaEvoltis.git

---
  
## Los tests cubren los siguientes handlers:

AddUserCommandHandler

DeleteUserCommandHandler

UpdateUserCommandHandler

SearchUserQueryHandler

Cada prueba valida el comportamiento en casos exitosos y de error.

---


Base de datos (Code-First)

La aplicación utiliza MySQL como base de datos.
Se sigue el enfoque Code-First, es decir, las tablas se generan a partir de las entidades definidas en el proyecto .NET.

## Configurar la base de datos

1.- Crear la base de datos vacía en MySQL:

 R.- CREATE DATABASE PruebaEvoltis;

2.- Configurar la cadena de conexión en appsettings.json
R.-
"ConnectionStrings": {
    "DefaultConnection": "server=localhost;user=root;password=TU_PASSWORD;database=PruebaEvoltis"
}

3.- Generar las tablas desde las entidades

# Crear la primera migración (si no existe)
R.- dotnet ef migrations add InitialCreat

# Aplicar la migración a la base de datos
R.- dotnet ef database update

Esto creará automáticamente todas las tablas correspondientes a tus entidades (Usuarios, Domicilios.).

-----


AUTOR
Fredy Claro Rojas