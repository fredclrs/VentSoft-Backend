using Application;
using Application.Interfaces;
using Application.Mappings;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Application (MediatR, comandos y queries)
builder.Services.AddApplicationServices();

// Infrastructure (DbContext + Services)
builder.Services.AddInfrastructureServices(builder.Configuration);

// AutoMapper: escanea todos los Profile del assembly de Application (no hace falta
// registrar cada uno a mano al agregar una entidad nueva).
builder.Services.AddAutoMapper(typeof(UsuarioProfile).Assembly);

// CORS: habilita al front (Vite) para consumir la API desde otro origen.
var corsAllowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
    ?? new[] { "http://localhost:5173" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins(corsAllowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Autenticación JWT
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection["Key"]!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSection["Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

builder.Services.AddAuthorization();

// Controllers: por defecto todos los endpoints requieren estar autenticado;
// se habilita el acceso anónimo puntualmente con [AllowAnonymous] (ver AuthController).
builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));

    // Sin esto, [ApiController] trata cualquier propiedad string no-nullable de los DTOs
    // (p.ej. Estado, que siempre lo calcula el servidor) como implícitamente obligatoria,
    // y devuelve 400 automático si el front no la manda. Los "obligatorio de verdad" ya
    // los cubren los FluentValidation validators de cada Command.
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API VentSoft",
        Description = "API de compra-venta e inventario para negocios pequeños",
        Contact = new OpenApiContact
        {
            Name = "VentSoft",
            Email = "soporte@ventsoft.com"
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Pegar solo el token JWT (sin el prefijo 'Bearer ').",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
});

var app = builder.Build();

// Semilla del primer usuario: sin esto nadie puede loguearse nunca (todos los
// endpoints requieren estar autenticado, incluido el que crea usuarios). Si la
// tabla Usuario está vacía, se crea un admin con contraseña conocida para poder
// entrar la primera vez; cambiarla apenas se ingresa.
using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        if (!context.Usuarios.Any())
        {
            var hash = passwordHasher.Hash("admin123");
            context.Usuarios.Add(new Usuario
            {
                Nombre = "Administrador",
                DocumentoIdentidad = "0",
                NombreUsuario = "admin",
                EsAdministrador = true,
                Contrasena = hash,
                ConfirmarContrasena = hash,
                Estado = "AC",
                FechaRegistro = DateTime.Now,
            });
            context.SaveChanges();

            Console.WriteLine("============================================================");
            Console.WriteLine(" Usuario administrador creado automáticamente (tabla vacía):");
            Console.WriteLine("   Usuario:    admin");
            Console.WriteLine("   Contraseña: admin123");
            Console.WriteLine(" Cambiala apenas inicies sesión la primera vez.");
            Console.WriteLine("============================================================");
        }
    }
    catch (Exception ex)
    {
        app.Logger.LogWarning(ex, "No se pudo verificar/crear el usuario administrador inicial.");
    }
}

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API VentSoft v1");
    });
}

// Manejo global de excepciones no atrapadas (los handlers de MediatR ya atrapan
// las suyas y devuelven BaseResponse.FailureResponse; esto cubre lo que se
// escape de ahí, por ejemplo errores de validación de FluentValidation).
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var feature = context.Features.Get<IExceptionHandlerFeature>();
        var ex = feature?.Error;

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = ex is FluentValidation.ValidationException ? 400 : 500;

        var mensaje = ex is FluentValidation.ValidationException validationEx
            ? string.Join(" | ", validationEx.Errors.Select(e => e.ErrorMessage))
            : "Ocurrió un error inesperado.";

        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            success = false,
            message = mensaje
        }));
    });
});

// En desarrollo NO se fuerza el redirect a HTTPS: si el front llama al puerto HTTP,
// un 307 en el preflight de CORS hace que el navegador lo bloquee directamente
// ("Redirect is not allowed for a preflight request"). En producción sí conviene.
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("FrontendPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
