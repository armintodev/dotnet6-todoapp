using dotnet6_training.Data;
using dotnet6_training.Data.Repository;
using dotnet6_training.Services.TodoService;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

#region Builder

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddFluentValidation();

builder.Services.AddDbContext<TodoContext>(_ =>
{
    _.UseSqlServer(builder.Configuration.GetConnectionString("TodoConnection"));
});

builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<ITodoService, TodoService>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Description = "Docs for my API", Version = "v1" });
});

builder.Services.BuildServiceProvider();

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

app.MapGet
    ("/todo", async (ITodoService service, CancellationToken cancellationToken) =>
    {
        var todos = await service.GetAll(cancellationToken);

        if (todos.ApiStatusCode is ApiStatusCode.Failed)
        {
            app.Logger.LogWarning(todos.Message);
            throw new Exception(todos.Message);
        }

        return todos.Data;
    });

app.MapGet
    ("/todo/{id}", async (ITodoService service, int id, CancellationToken cancellationToken) =>
    {
        var todo = await service.Find(id, cancellationToken);

        if (todo.ApiStatusCode is ApiStatusCode.Failed)
        {
            app.Logger.LogWarning(todo.Message);
            throw new Exception(todo.Message);
        }

        return todo.Data;
    });

app.MapPost("/todo", async (ITodoService service, [FromForm] CreateTodoRequest request, CancellationToken cancellationToken) =>
{
    var todo = await service.New(request, cancellationToken);

    if (todo.ApiStatusCode is ApiStatusCode.Failed)
    {
        app.Logger.LogWarning(todo.Message);
    }

    return todo;
});

app.MapPut("/todo", async (ITodoService service, [FromForm] EditTodoRequest request, CancellationToken cancellationToken) =>
{
    var todo = await service.Modify(request, cancellationToken);

    if (todo.ApiStatusCode is ApiStatusCode.Failed)
    {
        app.Logger.LogWarning(todo.Message);
    }

    return todo;
});

app.MapDelete("/todo", async (ITodoService service, int id, CancellationToken cancellationToken) =>
{
    var todo = await service.Delete(id, cancellationToken);

    if (todo.ApiStatusCode is ApiStatusCode.Failed)
    {
        app.Logger.LogWarning(todo.Message);
    }

    return todo;
});

await app.RunAsync();

#endregion