using dotnet6_training.Data;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

#region Builder

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<TodoContext>(_ =>
{
    _.UseSqlServer(builder.Configuration.GetConnectionString("TodoConnection"));
});

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
    ("/todo", async (TodoContext context, CancellationToken cancellationToken) =>
    {
        await Task.Delay(1000);
        var todos = await context.Todos.OrderByDescending(_ => _.CreateDate).ToListAsync(cancellationToken);
        List<TodoResponse> response = todos.ToResponse();
        return response;
    });

app.MapGet
    ("/todo/{id}", async (TodoContext context, int id) =>
    {
        await Task.Delay(1000);
        var todo = await context.Todos.FindAsync(id);
        if (todo is null) throw new ArgumentNullException("The todo model not found");
        TodoResponse response = todo;
        return response;
    });

app.MapPost("/todo", async (TodoContext context, [FromForm] CreateTodoRequest request, CancellationToken cancellationToken) =>
{
    await Task.Delay(1000);
    await context.Todos.AddAsync(request, cancellationToken);
    await context.SaveChangesAsync();
});

app.MapPut("/todo", async (TodoContext context, [FromForm] EditTodoRequest request, CancellationToken cancellationToken) =>
{
    await Task.Delay(1000);
    var todo = await context.Todos.FindAsync(request.Id);
    if (todo is null) throw new ArgumentNullException("The todo model not found");
    todo.Edit(request.Title, request.Description);
    await context.SaveChangesAsync();

    TodoResponse response = todo;

    return response;
});

app.MapDelete("/todo", async (TodoContext context, int id, CancellationToken cancellationToken) =>
{
    await Task.Delay(1000);
    var todo = await context.Todos.FindAsync(id);
    if (todo is null) throw new ArgumentNullException("The todo model not found");
    context.Todos.Remove(todo);
    await context.SaveChangesAsync();
});

await app.RunAsync();

#endregion