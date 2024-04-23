using Confluent.Kafka;
using Serilog;
using Microsoft.EntityFrameworkCore;
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

builder.Services.AddControllers();

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

app.UseMiddleware<RequestLoggingMiddleware>();
app.MapControllers();

app.Run();