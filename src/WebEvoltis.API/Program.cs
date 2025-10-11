using Application.Mappings;
using AutoMapper;
using Microsoft.OpenApi.Models;
using PruebaEvoltis.Application;
using PruebaEvoltis.Infrastructure;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Application (MediatR, comandos y queries)
builder.Services.AddApplicationServices();

// Infrastructure (DbContext + Services)
builder.Services.AddInfrastructureServices(builder.Configuration);

// AutoMapper
var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<UsuarioProfile>();    
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton<IMapper>(mapper);

// Controllers
builder.Services.AddControllers();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "API Prueba Evoltis",
        Description = "API para gestión de usuarios y operaciones CRUD",
        Contact = new OpenApiContact
        {
            Name = "Evoltis",
            Email = "soporte@evoltis.com"
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Prueba Evoltis v1");
    });
}

app.UseHttpsRedirection();

app.MapControllers();
app.Run();