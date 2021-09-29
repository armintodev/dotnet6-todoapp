using dotnet6_training.Models.Enums;
using dotnet6_training.Models.OperationResult;

namespace dotnet6_training.Data.Repository;

public class TodoRepository : ITodoRepository
{
    private readonly TodoContext _context;
    public TodoRepository(TodoContext context)
    {
        _context = context;
    }

    public IResult<IQueryable<Todo>> Get()
    {
        return new Result<IQueryable<Todo>>(_context.Todos).StatusCode(ApiStatusCode.Success).ToResult();
    }

    public async Task<IResult<Todo>> Get(int todoId, CancellationToken cancellationToken)
    {
        return new Result<Todo>(await _context.Todos.FindAsync(todoId, cancellationToken)).StatusCode(ApiStatusCode.Success).ToResult();
    }

    public async Task<IResult<Todo>> Add(Todo todo, CancellationToken cancellationToken)
    {
        var operation = await _context.Todos.AddAsync(todo, cancellationToken);
        await this.Save(cancellationToken);

        return new Result<Todo>(operation.Entity).StatusCode(ApiStatusCode.Success).ToResult();
    }

    public async Task<IResult<Todo>> Update(Todo todo, CancellationToken cancellationToken)
    {
        var updatedTodo = _context.Todos.Update(todo);
        await this.Save(cancellationToken);

        return new Result<Todo>(updatedTodo.Entity).StatusCode(ApiStatusCode.Success).ToResult();
    }

    public async Task<IResult> Delete(Todo todo, CancellationToken cancellationToken)
    {
        _context.Todos.Remove(todo);
        await this.Save(cancellationToken);

        return new Result().StatusCode(ApiStatusCode.Success).ToResult();
    }

    public async Task Save(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}