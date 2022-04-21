using dotnet6_training.Data;
using dotnet6_training.Data.Repository;
using dotnet6_training.Models.Configuration;
using dotnet6_training.Services.CacheService;
using dotnet6_training.Services.TodoService;

using FluentValidation.AspNetCore;

using Hangfire;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

#region Builder

builder.Services.Configure<CacheSetting>(configuration.GetSection(ConfigurationConstants.CACHE_SETTING));

services.AddControllers();
services.AddEndpointsApiExplorer();

services.AddFluentValidation();

services.AddDbContext<TodoContext>(_ =>
{
    _.UseSqlServer(configuration.GetConnectionString(ConfigurationConstants.SQL_SERVER_CONNECTION_STRING));
});

services.AddMemoryCache();

services.AddScoped<ITodoRepository, TodoRepository>();
services.AddScoped<ITodoService, TodoService>();

services.AddTransient<MemoryCacheService>();
services.AddTransient<RedisCacheService>();

services.AddTransient<Func<CacheSide, ICacheService>>(serviceProvider => side =>
{
    return side.Tech switch
    {
        ConfigurationConstants.MEMORY => serviceProvider.GetService<MemoryCacheService>(),
        ConfigurationConstants.REDIS => serviceProvider.GetService<RedisCacheService>(), //Redis Service not yet implemented
        _ => serviceProvider.GetService<MemoryCacheService>(),
    };
});

services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Description = "Docs for my API", Version = "v1" });
});

services.AddHangfire(_ => _.UseSqlServerStorage(configuration.GetConnectionString(ConfigurationConstants.SQL_SERVER_CONNECTION_STRING)));
services.AddHangfireServer();

services.AddCors(_ =>
{
    _.AddPolicy("Js", policy =>
    {
        policy.WithOrigins("http://127.0.0.1:5500")
        .AllowAnyHeader()
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

#endregion

await using var app = builder.Build();

#region Applicaiton, Middlewares

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

app.UseHangfireDashboard("/jobs");

app.UseCors("Js");

app.MapGet
    ("/todoAsync", async (ITodoService service, CancellationToken cancellationToken) =>
    {
        var todos = await service.ReadOnlyGetAll(cancellationToken);

        if (todos.ApiStatusCode is ApiStatusCode.Failed)
        {
            app.Logger.LogWarning(todos.Message);
            return Results.BadRequest(todos.Message);
        }

        return Results.Ok(todos.Data);
    });

app.MapGet
    ("/todo", async (ITodoService service, CancellationToken cancellationToken) =>
    {
        var todos = await service.GetAll(cancellationToken);

        if (todos.ApiStatusCode is ApiStatusCode.Failed)
        {
            app.Logger.LogWarning(todos.Message);
            return Results.BadRequest(todos.Message);
        }

        return Results.Ok(todos.Data);
    });

app.MapGet
    ("/todo/{id}", async (ITodoService service, int id, CancellationToken cancellationToken) =>
    {
        var todo = await service.Find(id, cancellationToken);

        if (todo.ApiStatusCode is ApiStatusCode.Failed)
        {
            app.Logger.LogWarning(todo.Message);
            return Results.BadRequest(todo.Message);
        }

        return Results.Ok(todo.Data);
    });

app.MapPost("/todo", async (ITodoService service, [FromForm] CreateTodoRequest request, CancellationToken cancellationToken) =>
{
    var todo = await service.New(request, cancellationToken);

    if (todo.ApiStatusCode is ApiStatusCode.Failed)
    {
        app.Logger.LogWarning(todo.Message);
        return Results.BadRequest(todo.Message);
    }

    return Results.Ok(todo);
});

app.MapPut("/todo", async (ITodoService service, [FromForm] EditTodoRequest request, CancellationToken cancellationToken) =>
{
    var todo = await service.Modify(request, cancellationToken);

    if (todo.ApiStatusCode is ApiStatusCode.Failed)
    {
        app.Logger.LogWarning(todo.Message);
        return Results.BadRequest(todo.Message);
    }

    return Results.Ok(todo);
});

app.MapDelete("/todo", async (ITodoService service, int id, CancellationToken cancellationToken) =>
{
    var todo = await service.Delete(id, cancellationToken);

    if (todo.ApiStatusCode is ApiStatusCode.Failed)
    {
        app.Logger.LogWarning(todo.Message);
        return Results.BadRequest(todo.Message);
    }

    return Results.Ok(todo);
});

await app.RunAsync();

#endregion