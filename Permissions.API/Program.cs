using System.Text;
using Confluent.Kafka;
using Microsoft.AspNetCore.Antiforgery;
using Serilog;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using Permissions.API.Configurations;
using Permissions.API.Mappings;
using Permissions.API.Middleware;
using Permissions.API.Services;
using Permissions.Infrastructure.Data.Contexts;
using Permissions.Infrastructure.Repositories;
using Permissions.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("PermissionContext")));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar la inyección de dependencias para las clases de configuración
builder.Services.Configure<ElasticsearchConfiguration>(builder.Configuration.GetSection("Elasticsearch"));
builder.Services.AddSingleton<ProducerConfig>(provider =>
            {
                var config = new ProducerConfig();
                config.BootstrapServers = builder.Configuration.GetSection("Kafka:BootstrapServers").Value;
                return config;
            });

builder.Services.AddSingleton<ConsumerConfig>(provider =>
            {
                var config = new ConsumerConfig();
                config.BootstrapServers = builder.Configuration.GetSection("Kafka:BootstrapServers").Value;
                config.GroupId = "permission-consumer-group";
                config.AutoOffsetReset = AutoOffsetReset.Earliest;
                return config;
            });

// AutoMapper configuration
builder.Services.AddAutoMapper(typeof(PermissionMappingProfile));

// Registrar los servicios necesarios para la aplicación
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IElasticsearchService, ElasticsearchService>();
// Registrar IKafkaProducerService con alcance de Scoped
builder.Services.AddScoped<IKafkaProducerService>(provider => 
    {
        // Obtener la configuración necesaria de la sección de configuración de la aplicación
        var bootstrapServers = builder.Configuration.GetSection("Kafka:BootstrapServers").Value;
        var topicName = builder.Configuration.GetSection("Kafka:TopicName").Value;

        // Crear una instancia de KafkaProducerService con los parámetros necesarios
        return new KafkaProducerService(bootstrapServers, topicName);
    });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "your_issuer",
            ValidAudience = "your_audience",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key"))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();

// Configuración del servicio Antiforgery
builder.Services.AddAntiforgery(options =>
    {
        options.HeaderName = "X-CSRF-TOKEN"; // Nombre del encabezado que contendrá el token CSRF
    });

// Serilog Logging
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<RequestLoggingMiddleware>();
// app.UseMiddleware<CsrfMiddleware>();  //Middleware personalizado, si uso este omito la configuracion del Antiforgery 
// Middleware Antiforgery
app.Use(next => context =>
{
    string path = context.Request.Path.Value;

    // Rutas que no requieren protección CSRF
    if (
        string.Equals(path, "/login", StringComparison.OrdinalIgnoreCase) ||
        string.Equals(path, "/register", StringComparison.OrdinalIgnoreCase)
       )
    {
        return next(context);
    }

    // Aplica el token CSRF a las solicitudes POST, PUT, DELETE y PATCH
    if (HttpMethods.IsPost(context.Request.Method) ||
        HttpMethods.IsPut(context.Request.Method) ||
        HttpMethods.IsDelete(context.Request.Method) ||
        HttpMethods.IsPatch(context.Request.Method))
    {
        var antiforgery = context.RequestServices.GetService<IAntiforgery>();
        var tokens = antiforgery.GetAndStoreTokens(context);
        context.Response.Cookies.Append("X-CSRF-TOKEN", tokens.RequestToken, new CookieOptions() { HttpOnly = false });
    }

    return next(context);
});
app.MapControllers();

app.Run();